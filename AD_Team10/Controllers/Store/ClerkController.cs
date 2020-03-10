using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.Authentication;
using PagedList;
using AD_Team10.ViewModels;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using AD_Team10.Service;
using System.Data.Entity;

//Authors: Phung Khanh Chi, Wang Wang Wang
namespace AD_Team10.Controllers.Store
{
    [CustomAuthorize(Roles = "CLERK")]
    public class ClerkController : Controller
    {
        static readonly int PAGE_SIZE = 8;
        static readonly string REQUISITION = "requisition";
        static readonly string ORDER = "order";
        private DBContext db = new DBContext();
        private RequisitionService reqService = new RequisitionService();
        private PurchaseOrderService orderService = new PurchaseOrderService();
        private ReportService reportService = new ReportService();

        public ActionResult Index()
        {
            ViewBag.requisitionList = db.Requisitions.Where(r => r.Status == Status.Approved || r.Status == Status.Incomplete || r.Status == Status.Completed).
                OrderByDescending(r => r.RequisitionDate).Take(PAGE_SIZE).ToList();
            ViewBag.orderList = db.PurchaseOrders.OrderByDescending(p => p.OrderDate).Take(PAGE_SIZE).ToList();
            ViewBag.voucherList = db.AdjustmentVouchers.OrderByDescending(v => v.AdjustmentDate).Take(PAGE_SIZE).ToList();
            ViewBag.stock = db.Items.OrderBy(i => i.UnitsInStock / i.ReorderLevel).Take(PAGE_SIZE).ToList();
            return View("~/Views/Store/Clerk/Index.cshtml");
        }

        //Author: Wang Wang Wang
        // GET: Pending requisition list
        public ActionResult GetPendingList(int? page, int? department, int? collectionPoint, string status = "")
        {
            var requisitions = reqService.GetPendingRequisitions().OrderByDescending(r => r.RequisitionDate).ToList();
            ViewBag.departments = reqService.GetDepartments();
            ViewBag.collectionPoints = reqService.GetCollectionPoints();
            ViewBag.statusList = new List<string> { Status.Approved.ToString(), Status.Incomplete.ToString() };
            if (department > 0) requisitions = requisitions.Where(r => r.Employee.DepartmentID == department).ToList();
            if (collectionPoint > 0) requisitions = requisitions.Where(r => r.Employee.Department.CollectionPointID == collectionPoint).ToList();
            if (status != "") requisitions = requisitions.Where(r => r.Status.ToString() == status).ToList();
            ViewBag.department = department;
            ViewBag.collectionPoint = collectionPoint;
            ViewBag.status = status;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/GetPendingList.cshtml", requisitions.ToPagedList(pageNumber, PAGE_SIZE));
        }


        // GET: View pending requisition detail
        public ActionResult ViewDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requisition requisition = reqService.Find(id);
            List<RequisitionDetail> requisitionDetail = requisition.RequisitionDetails.ToList();
            if (requisition == null)
            {
                return HttpNotFound();
            }
            //return View(requisition);
            return View("~/Views/Store/Clerk/ViewDetails.cshtml", Tuple.Create(requisition, requisitionDetail));
        }

        [HttpGet]
        public JsonResult GetRequisitionDetailsList()
        {
            List<RequisitionDetail> requisitionDetailsList = reqService.GetPendingRequisitionDetails();
            return Json(requisitionDetailsList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewRequisitionHistory(int? page, int? department, int? collectionPoint)
        {
            var requisitions = reqService.GetRequisitionByStatus(Status.Completed).OrderByDescending(r => r.RequisitionDate).ToList();
            ViewBag.departments = reqService.GetDepartments();
            ViewBag.collectionPoints = reqService.GetCollectionPoints();
            if (department > 0) requisitions = requisitions.Where(r => r.Employee.DepartmentID == department).ToList();
            if (collectionPoint > 0) requisitions = requisitions.Where(r => r.Employee.Department.CollectionPointID == collectionPoint).ToList();
            ViewBag.department = department;
            ViewBag.collectionPoint = collectionPoint;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/ViewRequisitionHistory.cshtml", requisitions.ToPagedList(pageNumber, PAGE_SIZE));
        }

        // GET: View retrieval list
        public ActionResult ViewRetrievalList()
        {
            int retrievalListId = reqService.FindCurrentRetrievalList().RetrievalListID;
            List<int> requisitionIDs = db.RequisitionRetrievals.Where(r => r.RetrievalListID == retrievalListId).Select(r => r.RequisitionID).ToList();
            var requisitionDetails = db.RequisitionDetails.Where(r => requisitionIDs.Contains(r.RequisitionID)).ToList();
            ViewData["requisitionDetails"] = requisitionDetails;
            Dictionary<Item, int> itemQuantity = reqService.GetItemAndQuantity(requisitionDetails);
            ViewData["itemquantity"] = itemQuantity;
            List<Item> items = itemQuantity.Keys.ToList();
            ViewData["items"] = items;
            ViewBag.retrievalListId = retrievalListId;
            Dictionary<Item, Dictionary<Requisition, int>> itemRquisitionList = new Dictionary<Item, Dictionary<Requisition, int>>();
            foreach (var item in items)
            {
                if (!itemRquisitionList.ContainsKey(item))
                    itemRquisitionList.Add(item, reqService.GetRequisitionByItem(item, requisitionDetails));
            }
            ViewData["itemRequisitionList"] = itemRquisitionList;
            return View("~/Views/Store/Clerk/ViewRetrievalList.cshtml", items);
        }

        //GET: Using Rerieval List table
        public ActionResult NewViewRetrievalList()
        {
            RetrievalList retrievalList = reqService.FindCurrentRetrievalList();   //GenerateRetrievalList() method in requisition service updated
            List<Item> items = reqService.GetDistictItemForRetrievalList(retrievalList);
            return View("~/Views/Store/Clerk/NewViewRetrievalList.cshtml", Tuple.Create(retrievalList, items));
        }

        public ActionResult GenerateDisbursement()
        {
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName");
            return View("~/Views/Store/Clerk/GenerateDisbursement.cshtml");
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(Models.Department department)
        {
            Dictionary<Item, int> departmentItemQuantity = reqService.GetDepartmentItemAndQuantity(department);
            ViewData["departmentItemAndQuantity"] = departmentItemQuantity;
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName", department.DepartmentID);
            return View("~/Views/Store/Clerk/GenerateDisbursement.cshtml", department);
        }

        public ActionResult UpdateRetrievalList(int id_retrieval)
        {
            RetrievalList retrievalList = db.RetrievalLists.SingleOrDefault(r => r.RetrievalListID == id_retrieval);
            List<Item> items = reqService.GetDistictItemForRetrievalList(retrievalList);
            ViewData["items"] = items;
            reqService.SequencedRetrievalList(items, retrievalList);
            reqService.UpdateRetrievalList(retrievalList);
            return View("~/Views/Store/Clerk/UpdateRetrievalList.cshtml", retrievalList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRetrievalList(RetrievalList retrievalList)
        {
            if (ModelState.IsValid)
            {
                RetrievalList editedRetrieval = reqService.GetRetrievalListById(retrievalList.RetrievalListID);
                List<Item> items = reqService.GetDistictItemForRetrievalList(editedRetrieval);
                reqService.SequencedRetrievalList(items, editedRetrieval);
                for (int i = 0; i < editedRetrieval.RetrievalListDetails.Count; i++)
                {
                    editedRetrieval.RetrievalListDetails[i].QuantityOffered = retrievalList.RetrievalListDetails[i].QuantityOffered;
                }
                reqService.UpdateRetrievalList(editedRetrieval);
                return RedirectToAction("NewViewRetrievalList");
            }
            return View("~/Views/Store/Clerk/UpdateRetrievalList.cshtml", retrievalList);
        }

        public ActionResult UpdateRequisitionDetails(int id_requisition)
        {
            Requisition requisition = reqService.GetRequisitionById(id_requisition);
            return View("~/Views/Store/Clerk/UpdateRequisitionDetails.cshtml", requisition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRequisitionDetails(Requisition requisition)
        {
            if (ModelState.IsValid)
            {
                Requisition editedRequisition = reqService.GetRequisitionById(requisition.RequisitionID);
                for (int i = 0; i < editedRequisition.RequisitionDetails.Count; i++)
                {
                    editedRequisition.RequisitionDetails.ToList()[i].QuantityReceived = requisition.RequisitionDetails.ToList()[i].QuantityReceived;
                }
                if (reqService.IsCompleted(editedRequisition))
                {
                    editedRequisition.CompletedDate = DateTime.Now;
                    editedRequisition.Status = Status.Completed;
                }
                else
                {
                    editedRequisition.Status = Status.Incomplete;
                    RetrievalList retrievalList = reqService.GetRetrievalListForNow();
                    reqService.IncompletedRequisitionTransferToRetrieval(editedRequisition, retrievalList);
                    reqService.UpdateRetrievalList(retrievalList);
                }
                reqService.UpdateRequisition(editedRequisition);
                return RedirectToAction("ViewDetails", new { id = requisition.RequisitionID });
            }
            return View("~/Views/Store/Clerk/UpdateRequisitionDetails.cshtml", requisition);
        }


        public ActionResult ViewOrderHistory(int? page, int? supplier, string status = "")
        {
            List<PurchaseOrder> purchaseOrders = reqService.GetPurchaseOrder().OrderByDescending(p => p.OrderDate).ToList();
            ViewBag.supplierList = db.Suppliers.ToList();
            ViewBag.statusList = new List<string> { OrderStatus.Pending.ToString(),
                    OrderStatus.Delivering.ToString(), OrderStatus.Incompleted.ToString(), OrderStatus.Completed.ToString()};
            if (supplier > 0) purchaseOrders = purchaseOrders.Where(p => p.SupplierID == supplier).ToList();
            if (status != "") purchaseOrders = purchaseOrders.Where(p => p.OrderStatus.ToString() == status).ToList();
            ViewBag.status = status;
            ViewBag.supplier = supplier;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/ViewOrderHistory.cshtml", purchaseOrders.ToPagedList(pageNumber, PAGE_SIZE));
        }


        public ActionResult ViewOrderDetails(int id)
        {
            List<PurchaseOrderDetail> purchaseOrderDetails = orderService.GetOrderDetailByOrderId(id);
            ViewData["purchaseOrder"] = orderService.GetPurchaseOrder(id);
            return View("~/Views/Store/Clerk/ViewOrderDetails.cshtml", purchaseOrderDetails);
        }


        //Author: Phung Khanh Chi
        public ActionResult UpdatePurchaseOrder(int orderID)
        {
            PurchaseOrder purchaseOrder = orderService.GetPurchaseOrder(orderID);
            return View("~/Views/Store/Clerk/UpdatePurchaseOrder.cshtml", purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                PurchaseOrder editedorder = orderService.GetPurchaseOrder(purchaseOrder.PurchaseOrderID);
                for (int i = 0; i < editedorder.PurchaseOrderDetails.Count; i++)
                {
                    editedorder.PurchaseOrderDetails.ToList()[i].QuantityReceived = purchaseOrder.PurchaseOrderDetails.ToList()[i].QuantityReceived;
                }
                if (orderService.IsComoleted(editedorder))
                {
                    editedorder.CompletedDate = DateTime.Now;
                    editedorder.OrderStatus = OrderStatus.Completed;
                }
                orderService.UpdatePurchaseOrder(editedorder);
                return RedirectToAction("ViewOrderDetails", new { id = editedorder.PurchaseOrderID });
            }
            return View("~/Views/Store/Clerk/UpdatePurchaseOrder.cshtml", purchaseOrder);
        }

        public ActionResult CreatePurchaseOrder()
        {
            var items = db.Items.Where(i => i.UnitsInStock < i.ReorderQuantity).ToList();
            List<int> pendingItems = db.PurchaseOrderDetails.Where(p => p.PurchaseOrder.OrderStatus == OrderStatus.Pending ||
                                                            p.PurchaseOrder.OrderStatus == OrderStatus.Delivering ||
                                                            (p.PurchaseOrder.OrderStatus == OrderStatus.Incompleted &&
                                                             ((p.Quantity - p.QuantityReceived) >= (p.Item.ReorderQuantity - p.Item.UnitsInStock))))
                                                             .Select(p => p.ItemID).Distinct().ToList();
            items = items.Where(i => !pendingItems.Contains(i.ItemID)).ToList();
            return View("~/Views/Store/Clerk/CreatePurchaseOrder.cshtml", items);
        }


        [HttpPost]
        public JsonResult CreatePurchaseOrder(List<NewPurchaseOrder> newPurchaseOrders)
        {
            List<PurchaseOrder> orders = newPurchaseOrders.GroupBy(p => p.SupplierID)
                                           .Select(p => new PurchaseOrder
                                           {
                                               SupplierID = p.Key,
                                               OrderDate = DateTime.Today,
                                               OrderStatus = OrderStatus.Pending
                                           }).ToList();
            for (int i = 0; i < orders.Count(); i++)
            {
                PurchaseOrder order = orders[i];
                db.PurchaseOrders.Add(order);
                int supplierId = order.SupplierID;
                List<PurchaseOrderDetail> orderDetails = newPurchaseOrders.Where(o => o.SupplierID == supplierId)
                                .Select(o => new PurchaseOrderDetail
                                {
                                    PurchaseOrder = order,
                                    ItemID = o.ItemID,
                                    Quantity = o.Quantity,
                                    Price = o.Price
                                }).ToList();
                foreach (var detail in orderDetails)
                {
                    db.PurchaseOrderDetails.Add(detail);
                }
            }
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Inventory(int? page, string stockLevel = "", string sort = "", string searchStr = "")
        {
            var items = db.Items.ToList();
            items = items.Where(i => i.Description.ToLower().Contains(searchStr.ToLower())).ToList();
            if (stockLevel == "low") items = items.Where(i => i.UnitsInStock < i.ReorderLevel).ToList();
            else if (stockLevel == "enough") items = items.Where(i => i.UnitsInStock >= i.ReorderLevel).ToList();
            else { items = items.ToList(); }
            switch (sort)
            {
                case "stockAsc":
                    items = items.OrderBy(i => i.UnitsInStock).ToList();
                    break;
                case "stockDesc":
                    items = items.OrderByDescending(i => i.UnitsInStock).ToList();
                    break;
                case "reorderAsc":
                    items = items.OrderBy(i => i.ReorderLevel).ToList();
                    break;
                case "reorderDesc":
                    items = items.OrderByDescending(i => i.ReorderLevel).ToList();
                    break;
                default:
                    items = items.ToList();
                    break;
            }
            ViewBag.searchStr = searchStr;
            ViewBag.stockLevel = stockLevel;
            ViewBag.sort = sort;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/Inventory.cshtml", items.ToPagedList(pageNumber, PAGE_SIZE));
        }

        public ActionResult AdjustmentVoucher(int? page, int? clerk, string status = "")
        {
            ViewBag.statusList = new List<string> { VoucherStatus.Pending.ToString(),
                    VoucherStatus.Rejected.ToString(), VoucherStatus.Supervisor_Approved.ToString(), VoucherStatus.Manager_Approved.ToString()};
            ViewBag.clerkList = db.AdjustmentVouchers.Select(a => a.Clerk).Distinct().ToList();
            var vouchers = db.AdjustmentVouchers.OrderBy(v => v.AdjustmentVoucherID).OrderByDescending(a => a.AdjustmentDate).ToList();
            if (status != "") vouchers = vouchers.Where(v => v.Status.ToString() == status).ToList();
            if (clerk > 0) vouchers = vouchers.Where(v => v.StoreEmployeeID == clerk).ToList();
            ViewBag.status = status;
            ViewBag.clerk = clerk;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/AdjustmentVoucher.cshtml", vouchers.ToPagedList(pageNumber, PAGE_SIZE));
        }

        public ActionResult VoucherDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentVoucher voucher = db.AdjustmentVouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Store/Clerk/VoucherDetails.cshtml", voucher);
        }


        public ActionResult CreateVoucher()
        {
            var item = db.Items;
            ViewBag.itemList = item.ToList();

            return View("~/Views/Store/Clerk/CreateVoucher.cshtml");
        }

        [HttpPost]
        public JsonResult CreateVoucher(List<AdjustmentVoucherDetail> newVoucherDetails)
        {
            if (newVoucherDetails == null)
            {
                return Json(0);
            }
            else
            {
                CustomPrincipal currenUser = (CustomPrincipal)System.Web.HttpContext.Current.User;
                AdjustmentVoucher newVoucher = new AdjustmentVoucher
                {
                    AdjustmentDate = DateTime.Now,
                    Status = VoucherStatus.Pending,
                    StoreEmployeeID = currenUser.UserID
                };
                db.AdjustmentVouchers.Add(newVoucher);
                db.SaveChanges();
                int voucherID = newVoucher.AdjustmentVoucherID;
                newVoucherDetails = CheckDuplicateInVoucher(newVoucherDetails);
                foreach (AdjustmentVoucherDetail newVoucherDetail in newVoucherDetails)
                {
                    newVoucherDetail.VoucherID = voucherID;
                    db.AdjustmentVoucherDetails.Add(newVoucherDetail);
                }
                int record = db.SaveChanges();
                return Json(voucherID);
            }
        }

        public List<AdjustmentVoucherDetail> CheckDuplicateInVoucher(List<AdjustmentVoucherDetail> voucherDetails)
        {
            var results = voucherDetails
                 .GroupBy(p => new { p.ItemID, p.Reason })
                 .Select(g => new AdjustmentVoucherDetail
                 {
                     ItemID = g.Key.ItemID,
                     Quantity = g.Sum(p => p.Quantity),
                     Reason = g.Key.Reason
                 })
                 .ToList();
            return results;
        }

        public ActionResult RequisitionReport()
        {
            var departments = db.Departments.ToList();
            var categories = db.Categories.ToList();
            ViewBag.departments = departments;
            ViewBag.categories = categories;
            return View("~/Views/Store/Clerk/RequisitionReport.cshtml");
        }

        public ActionResult OrderReport()
        {
            var categories = db.Categories.ToList();
            ViewBag.categories = categories;
            return View("~/Views/Store/Clerk/OrderReport.cshtml");
        }

        [HttpPost]
        public ActionResult RequisitionReport(RequisitionReport reqReport)
        {
            Session["reqReport"] = reqReport;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderReport(OrderReport orderReport)
        {
            Session["orderReport"] = orderReport;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowReqReport()
        {
            RequisitionReport reqReport = Session["reqReport"] as RequisitionReport;
            if (reqReport.CategoryList == null)
                reqReport.CategoryList = db.Categories.Select(c => c.CategoryId).ToList();

            if (reqReport.DepartmentList == null)
                reqReport.DepartmentList = db.Departments.Select(d => d.DepartmentID).ToList();

            ViewBag.report = reqReport;
            reportService.GetRequisitionData(reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt);
            if (timeDt.Count() == 0 || catDt.Count() == 0)
            {
                ViewBag.message = "The department(s) did not made any requisition for these categories during this period";
                return PartialView("~/Views/Shared/_Report.cshtml", null);
            }
            else
            {
                DataTable dt = reportService.GetTable(categories, reqReport.DepartmentList, out int[] catSize,
                    out string tablePeriod, timeDt, reqReport.StartDate, reqReport.EndDate, REQUISITION);
                ViewBag.catSize = catSize;
                ViewBag.tableName = "Department requisition quantity by category from " + tablePeriod;
                return PartialView("~/Views/Shared/_Report.cshtml", dt);
            }
        }

        public ActionResult ShowOrderReport()
        {
            OrderReport orderReport = Session["orderReport"] as OrderReport;
            if (orderReport.CategoryList == null)
                orderReport.CategoryList = db.Categories.Select(c => c.CategoryId).ToList();
            ViewBag.report = orderReport;
            reportService.GetOrderData(orderReport, out string[] categories, out ReportByTimeSeries[] timeDt);
            DataTable dt = reportService.GetTable(categories, null, out int[] catSize, out string tablePeriod,
                timeDt, orderReport.StartDate, orderReport.EndDate, ORDER);
            if (timeDt.Count() == 0)
            {
                ViewBag.message = "The store did not made any order for these categories during this period";
                return PartialView("~/Views/Shared/_Report.cshtml", null);
            }
            else
            {
                ViewBag.catSize = catSize;
                ViewBag.tableName = "Purchase order quantity by category from " + tablePeriod;
                return PartialView("~/Views/Shared/_Report.cshtml", dt);
            }
        }


        public void ExportReqDataTable(string report)
        {
            RequisitionReport reqReport = reportService.GetReqReport(report);
            reportService.GetRequisitionData(reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt);
            DataTable dt = reportService.GetTable(categories, reqReport.DepartmentList, out int[] catSize,
                out string tablePeriod, timeDt, reqReport.StartDate, reqReport.EndDate, REQUISITION);
            string fileName = "Requisition " + tablePeriod.Replace('/', '-');
            ExportData(dt, fileName, catSize, categories);
        }

        public void ExportOrderDataTable(string report)
        {
            OrderReport orderReport = reportService.GetOrderReport(report);
            reportService.GetOrderData(orderReport, out string[] categories, out ReportByTimeSeries[] timeDt);
            DataTable dt = reportService.GetTable(categories, null, out int[] catSize, out string tablePeriod,
                timeDt, orderReport.StartDate, orderReport.EndDate, ORDER);
            DateTime endDate = Convert.ToDateTime(orderReport.EndDate);
            string fileName = "PurchaseOrder " + tablePeriod.Replace('/', '-');
            ExportData(dt, fileName, catSize, categories);
        }

        public void ExportData(DataTable dt, string fileName, int[] catSize, string[] categories)
        {
            reportService.ExportData(dt, fileName, catSize, categories);
        }


        public ActionResult ShowRequisitionChart(string report, string type, string groupBy)
        {
            MemoryStream imgStream = reportService.ShowRequisitionChart(report, type, groupBy);
            return File(imgStream, "image/png");
        }

        public ActionResult ShowOrderChart(string report, string type, string groupBy)
        {
            MemoryStream imgStream = reportService.ShowOrderChart(report, type, groupBy);
            return File(imgStream, "image/png");
        }


        public ActionResult DepartmentList()
        {
            List<Models.Department> departments = db.Departments.ToList();
            return View("~/Views/Store/Clerk/DepartmentList.cshtml", departments);
        }

        public ActionResult ItemCatalogue(int? page, string sort = "", string searchStr = "")
        {
            var items = db.Items.ToList();
            items = items.Where(i => i.Description.ToLower().Contains(searchStr.ToLower())).ToList();
            switch (sort)
            {
                case "reorderAsc":
                    items = items.OrderBy(i => i.ReorderLevel).ToList();
                    break;
                case "reorderDesc":
                    items = items.OrderByDescending(i => i.ReorderLevel).ToList();
                    break;
                default:
                    items = items.ToList();
                    break;
            }
            ViewBag.searchStr = searchStr;
            ViewBag.sort = sort;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/ItemCatalogue.cshtml", items.ToPagedList(pageNumber, PAGE_SIZE));
        }

        public ActionResult SupplierList(int? page)
        {
            var suppliers = db.Suppliers.ToList();
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/SupplierList.cshtml", suppliers.ToPagedList(pageNumber, PAGE_SIZE));
        }


        public ActionResult DeleteSupplier(int supplierId, int page)
        {
            Supplier supplier = db.Suppliers.SingleOrDefault(s => s.SupplierID == supplierId);
            var supplierItems = db.SupplierItems.Where(s => s.SupplierID == supplierId);
            var purchaseOrders = db.PurchaseOrders.Where(p => p.SupplierID == supplierId);
            foreach (var order in purchaseOrders)
            {
                var details = order.PurchaseOrderDetails.ToList();
                foreach (var detail in details)
                {
                    db.PurchaseOrderDetails.Remove(detail);
                }
                db.PurchaseOrders.Remove(order);
            }

            foreach (var item in supplierItems)
            {
                db.SupplierItems.Remove(item);
            }
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("SupplierList", new { page });
        }

        public ActionResult DeleteItem(int itemId, int page)
        {
            Item item = db.Items.SingleOrDefault(t => t.ItemID == itemId);
            var supplierItems = db.SupplierItems.Where(s => s.ItemID == itemId);
            var purchaseOrderDetails = db.PurchaseOrderDetails.Where(p => p.ItemID == itemId);
            var requisitionDetails = db.RequisitionDetails.Where(r => r.ItemID == itemId);
            var retrievalListDetails = db.RetrievalListDetails.Where(r => r.ItemID == itemId);
            var adjustmentVoucherDetails = db.AdjustmentVoucherDetails.Where(r => r.ItemID == itemId);
            foreach (var s in supplierItems)
            {
                db.SupplierItems.Remove(s);
            }

            foreach (var p in purchaseOrderDetails)
            {
                db.PurchaseOrderDetails.Remove(p);
            }
            foreach (var r in requisitionDetails)
            {
                db.RequisitionDetails.Remove(r);
            }
            foreach (var r in retrievalListDetails)
            {
                db.RetrievalListDetails.Remove(r);
            }
            foreach (var p in adjustmentVoucherDetails)
            {
                db.AdjustmentVoucherDetails.Remove(p);
            }
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("ItemCatalogue", new { page });
        }


        public ActionResult EditItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName", item.CategoryID);
            return View("~/Views/Store/Clerk/EditItem.cshtml", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem([Bind(Include = "ItemID,Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ItemDetails", new { id = item.ItemID });
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName", item.CategoryID);
            return View("~/Views/Store/Clerk/EditItem.cshtml", item);
        }


        public ActionResult ItemDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Store/Clerk/ItemDetails.cshtml", item);
        }

        public ActionResult CreateItem()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View("~/Views/Store/Clerk/CreateItem.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem([Bind(Include = "ItemID,Description,UnitOfMeasure,ReorderLevel,ReorderQuantity,UnitsInStock,CategoryID")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("ItemDetails", new { id = item.ItemID });
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName", item.CategoryID);
            return View("~/Views/Store/Clerk/CreateItem.cshtml", item);
        }

        public ActionResult SupplierDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Store/Clerk/SupplierDetails.cshtml", supplier);
        }

        // GET: Suppliers/Create
        public ActionResult CreateSupplier()
        {
            return View("~/Views/Store/Clerk/CreateSupplier.cshtml");
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSupplier([Bind(Include = "SupplierID,SupplierCode,SupplierName,ContactName,Phone,Fax,Address,GSTNumber")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("SupplierDetails", new { id = supplier.SupplierID });
            }

            return View("~/Views/Store/Clerk/CreateSupplier.cshtml", supplier);
        }

        public ActionResult EditSupplier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Store/Clerk/EditSupplier.cshtml", supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSupplier([Bind(Include = "SupplierID,SupplierCode,SupplierName,ContactName,Phone,Fax,Address,GSTNumber")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SupplierDetails", new { id = supplier.SupplierID });
            }
            return View("~/Views/Store/Clerk/EditSupplier.cshtml", supplier);
        }
    }
}