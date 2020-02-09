using AD_Team10.DAL;
using AD_Team10.DataService;
using AD_Team10.Models;
using AD_Team10.SelectListModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers
{
    public class StoreClerkController : Controller
    {
        private RequisitionService reqService = new RequisitionService();
        private DBContext db = new DBContext();
        private PurchaseOrderService orderService = new PurchaseOrderService();

        // GET: StoreClerk
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pending requisition list
        public ActionResult GetPendingList()
        {
            var requisitions = reqService.GetPendingRequisitions();
            var department = new Department();
            var collectionPoint = new CollectionPoint();
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName");
            ViewBag.CollectionPointID = new SelectList(reqService.GetCollectionPoints(), "CollectionPointID", "CollectionPointName");
            return View(Tuple.Create(requisitions,department,collectionPoint));
        }

        [HttpPost]
        public ActionResult GetPendingList(Department department, CollectionPoint collectionPoint)
        {
            var requisitions = reqService.GetPendingRequisitions();
            ViewBag.CollectionPointID = new SelectList(reqService.GetCollectionPoints(), "CollectionPointID", "CollectionPointName", collectionPoint.CollectionPointID);
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName", department.DepartmentID);
            if (department != null)
            {
                if (department.DepartmentID == 0)
                {
                    if(collectionPoint != null)
                    {
                        if(collectionPoint.CollectionPointID == 0)
                        {
                            return View(Tuple.Create(requisitions.ToList(), new Department(), new CollectionPoint()));
                        }
                        else
                        {
                            requisitions = requisitions.Where(x => x.Employee.Department.CollectionPointID == collectionPoint.CollectionPointID).ToList();
                            return View(Tuple.Create(requisitions, department, collectionPoint));
                        }
                    }
                    else
                    {
                        return View(Tuple.Create(requisitions.ToList(), new Department(), new CollectionPoint()));
                    }
                }
                else
                {
                    requisitions = requisitions.Where(x => x.Employee.DepartmentID == department.DepartmentID).ToList();
                    return View(Tuple.Create(requisitions, department, collectionPoint));
                }
            }
            else
            {
                return View(Tuple.Create(requisitions, department, collectionPoint));
            }
                
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
            return View(Tuple.Create(requisition, requisitionDetail));
        }

        [HttpGet]
        public JsonResult GetRequisitionDetailsList()
        {
            List<RequisitionDetail> requisitionDetailsList = reqService.GetPendingRequisitionDetails();
            return Json(requisitionDetailsList, JsonRequestBehavior.AllowGet);
        }

        // GET: View requisition history
        public ActionResult ViewRequisitionHistory()
        {
            var requisitions = reqService.GetRequisitionByStatus(Status.Completed);
            var department = new Department();
            var collectionPoint = new CollectionPoint();
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName");
            ViewBag.CollectionPointID = new SelectList(reqService.GetCollectionPoints(), "CollectionPointID", "CollectionPointName");
            return View(Tuple.Create(requisitions, department, collectionPoint));
        }

        [HttpPost]
        public ActionResult ViewRequisitionHistory(Department department, CollectionPoint collectionPoint)
        {
            var requisitions = reqService.GetRequisitionByStatus(Status.Completed);
            ViewBag.CollectionPointID = new SelectList(reqService.GetCollectionPoints(), "CollectionPointID", "CollectionPointName", collectionPoint.CollectionPointID);
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName", department.DepartmentID);
            if (department != null)
            {
                if (department.DepartmentID == 0)
                {
                    if (collectionPoint != null)
                    {
                        if (collectionPoint.CollectionPointID == 0)
                        {
                            return View(Tuple.Create(requisitions.ToList(), new Department(), new CollectionPoint()));
                        }
                        else
                        {
                            requisitions = requisitions.Where(x => x.Employee.Department.CollectionPointID == collectionPoint.CollectionPointID).ToList();
                            return View(Tuple.Create(requisitions, department, collectionPoint));
                        }
                    }
                    else
                    {
                        return View(Tuple.Create(requisitions.ToList(), new Department(), new CollectionPoint()));
                    }
                }
                else
                {
                    requisitions = requisitions.Where(x => x.Employee.DepartmentID == department.DepartmentID).ToList();
                    return View(Tuple.Create(requisitions, department, collectionPoint));
                }
            }
            else
            {
                return View(Tuple.Create(requisitions, department, collectionPoint));
            }
        }

        // GET: View retrieval list
        public ActionResult ViewRetrievalList()
        {
            var requisitionDetails = reqService.GetPendingRequisitionDetails();
            ViewData["requisitionDetails"] = requisitionDetails;
            Dictionary<Item, int> itemQuantity = reqService.GetItemAndQuantity(requisitionDetails);
            ViewData["itemquantity"] = itemQuantity;
            List<Item> items = reqService.GetDistinctItem(requisitionDetails);
            ViewData["items"] = items;
            
            Dictionary<Item, Dictionary<Requisition, int>> itemRquisitionList = new Dictionary<Item, Dictionary<Requisition, int>>();
            foreach (var item in items)
            {
                if (!itemRquisitionList.ContainsKey(item))
                    itemRquisitionList.Add(item, reqService.GetRequisitionByItem(item, requisitionDetails));
            }
            ViewData["itemRequisitionList"] = itemRquisitionList;
            return View(items);
        }

        //GET: Using Rerieval List table
        public ActionResult NewViewRetrievalList()
        {
            RetrievalList retrievalList = reqService.GetRetrievalListForNow();   //After test, change to generate retrieval list
            List<Item> items = reqService.GetDistictItemForRetrievalList(retrievalList);
            return View(Tuple.Create(retrievalList, items));
        }

        public ActionResult GenerateDisbursement()
        {
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName");
            return View();
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(Department department)
        {
            Dictionary<Item, int> departmentItemQuantity = reqService.GetDepartmentItemAndQuantity(department);
            ViewData["departmentItemAndQuantity"] = departmentItemQuantity;
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName", department.DepartmentID);
            return View(department);
        }

        public ActionResult UpdateRetrievalList(int id_retrieval)
        {
            RetrievalList retrievalList = reqService.GetRetrievalListForNow();   //After test, change to generate retrieval list
            List<Item> items = reqService.GetDistictItemForRetrievalList(retrievalList);
            ViewData["items"] = items;
            return View(retrievalList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRetrievalList(RetrievalList retrievalList)
        {
            if (ModelState.IsValid)
            {
                RetrievalList editedRetrieval = reqService.GetRetrievalListById(retrievalList.RetrievalListID);
                foreach(var details in retrievalList.RetrievalListDetails)
                {
                    editedRetrieval.RetrievalListDetails.Where(x => x.ItemID == details.ItemID && x.DepartmentID == details.DepartmentID)
                                                        .SingleOrDefault().QuantityOffered = details.QuantityOffered;
                }
                reqService.UpdateRetrievalList(editedRetrieval);
                return RedirectToAction("NewViewRetrievalList");
            }
            return View(retrievalList);
        }

        public ActionResult UpdateRequisitionDetails(int id_requisition)
        {
            Requisition requisition = reqService.GetRequisitionById(id_requisition);
            return View(requisition);
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
                    editedRequisition.RequisitionDetails[i].QuantityReceived = requisition.RequisitionDetails[i].QuantityReceived;
                }
                if (reqService.IsCompleted(editedRequisition))
                {
                    editedRequisition.CompletedDate = DateTime.Now;
                    editedRequisition.Status = Status.Completed;
                }
                else
                {
                    editedRequisition.Status = Status.Incompleted;
                    RetrievalList retrievalList = reqService.GetRetrievalListForNow();
                    reqService.IncompletedRequisitionTransferToRetrieval(editedRequisition, retrievalList);
                    reqService.UpdateRetrievalList(retrievalList);
                }
                reqService.UpdateRequisition(editedRequisition);
                return RedirectToAction("ViewDetails", new { id = requisition.RequisitionID });
            }
            return View(requisition);
        }

        public ActionResult CreateNewOrder()
        {
            ViewBag.SupplierId = new SelectList(orderService.GetSuppliersList(), "SupplierID", "SupplierName");
            return View();
        }

        public ActionResult GetPendingOrderList()
        {
            List<PurchaseOrder> purchaseOrders = reqService.GetPendingPurchaseOrder();
            Supplier supplier = new Supplier();
            SelectOrderStatus selectOrderStatus = new SelectOrderStatus();
            ViewBag.SupplierID = new SelectList(orderService.GetSuppliersList(), "SupplierID", "SupplierName");
            ViewBag.OrderStatusId = new SelectList(selectOrderStatus.GetSelectOrderList(), "OrderStatusId", "OrderStatusName");
            return View(Tuple.Create(purchaseOrders, supplier, selectOrderStatus));
        }

        [HttpPost]
        public ActionResult GetPendingOrderList(Supplier supplier, SelectOrderStatus selectOrderStatus)
        {
            List<PurchaseOrder> purchaseOrders = reqService.GetPendingPurchaseOrder();
            ViewBag.SupplierID = new SelectList(orderService.GetSuppliersList(), "SupplierID", "SupplierName", supplier.SupplierID);
            ViewBag.OrderStatusId = new SelectList(selectOrderStatus.GetSelectOrderList(), "OrderStatusId", "OrderStatusName", selectOrderStatus.OrderStatusId);
            switch (selectOrderStatus.OrderStatusId)
            {
                case 1:
                    selectOrderStatus.OrderStatusName = "Pending";
                    break;
                case 2:
                    selectOrderStatus.OrderStatusName = "Delivering";
                    break;
                case 3:
                    selectOrderStatus.OrderStatusName = "Incompleted";
                    break;
                case 4:
                    selectOrderStatus.OrderStatusName = "Completed";
                    break;
                default:
                    break;
            }
            if (supplier != null)
            {
                if (supplier.SupplierID == 0)
                {
                    if (selectOrderStatus.OrderStatusId == 0)
                    {
                        return View(Tuple.Create(purchaseOrders, supplier, selectOrderStatus));
                    }
                    else
                    {
                        purchaseOrders = purchaseOrders.Where(x => x.OrderStatus.ToString() == selectOrderStatus.OrderStatusName).ToList();
                        return View(Tuple.Create(purchaseOrders, supplier, selectOrderStatus));
                    }
                }
                else
                {
                    if (selectOrderStatus.OrderStatusId != 0)
                    {
                        purchaseOrders = purchaseOrders.Where(x => x.OrderStatus.ToString() == selectOrderStatus.OrderStatusName && x.SupplierID == supplier.SupplierID).ToList();
                        return View(Tuple.Create(purchaseOrders, supplier, selectOrderStatus));
                    }
                    else
                    {
                        purchaseOrders = purchaseOrders.Where(x => x.SupplierID == supplier.SupplierID).ToList();
                        return View(Tuple.Create(purchaseOrders, supplier, selectOrderStatus));
                    }
                }
            }
            else
            {
                return View(Tuple.Create(purchaseOrders, supplier, selectOrderStatus));
            }
        }

        public ActionResult ViewOrderHistory()
        {
            List<PurchaseOrder> purchaseOrders = orderService.GetPurchaseOrderHistory();
            Supplier supplier = new Supplier();
            ViewBag.SupplierID = new SelectList(orderService.GetSuppliersList(), "SupplierID", "SupplierName");
            return View(Tuple.Create(purchaseOrders, supplier));
        }

        [HttpPost]
        public ActionResult ViewOrderHistory(Supplier supplier)
        {
            List<PurchaseOrder> purchaseOrders = orderService.GetPurchaseOrderHistory();
            ViewBag.SupplierID = new SelectList(orderService.GetSuppliersList(), "SupplierID", "SupplierName", supplier.SupplierID);
            if (supplier != null)
            {
                if (supplier.SupplierID == 0)
                {
                    return View(Tuple.Create(purchaseOrders, supplier));
                }
                else
                {
                    purchaseOrders = purchaseOrders.Where(x => x.SupplierID == supplier.SupplierID).ToList();
                    return View(Tuple.Create(purchaseOrders, supplier));
                }
            }
            else
            {
                return View(Tuple.Create(purchaseOrders, supplier));
            }
        }

        public ActionResult ViewOrderDetails(int id)
        {
            List<PurchaseOrderDetail> purchaseOrderDetails = orderService.GetOrderDetailByOrderId(id);
            ViewData["purchaseOrder"] = orderService.GetPurchaseOrder(id);
            return View(purchaseOrderDetails);
        }

        public ActionResult UpdatePurchaseOrder(int orderID)
        {
            PurchaseOrder purchaseOrder = orderService.GetPurchaseOrder(orderID);
            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                PurchaseOrder editedorder = orderService.GetPurchaseOrder(purchaseOrder.PurchaseOrderID);
                for(int i = 0; i<editedorder.PurchaseOrderDetails.Count; i++)
                {
                    editedorder.PurchaseOrderDetails[i].QuantityReceived = purchaseOrder.PurchaseOrderDetails[i].QuantityReceived;
                }
                if (orderService.IsComoleted(editedorder))
                {
                    editedorder.CompletedDate = DateTime.Now;
                    editedorder.OrderStatus = OrderStatus.Completed;
                }
                orderService.UpdatePurchaseOrder(editedorder);
                return RedirectToAction("ViewOrderDetails", new { id = editedorder.PurchaseOrderID });
            }
            return View(purchaseOrder);
        }
    }
}