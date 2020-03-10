using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.Service;
using AD_Team10.ViewModels;

//Author: Stephanie Cheng Sin Yin, Phung Khanh Chi
namespace AD_Team10.Controllers.Department
{
    public class HeadController : Controller
    {
        private DBContext db = new DBContext();
        private HeadService headService = new HeadService();
        private ReportService reportService = new ReportService();
        static readonly string REQUISITION = "requisition";

        //Author: Stephanie Cheng Sin Yin
        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult Index()
        {
            int headID = headService.FindUserID();
            int depID = headService.FindDepID();
            int actingHeadID = headService.FindActingHeadID(depID);

            if (actingHeadID != 0)
            {
                DeptUser depUser = db.DeptUsers.Where(x => x.DeptEmployeeID == actingHeadID).First();

                DateTime? endDate = db.DeptUsers.Where(x => x.DeptEmployeeID == actingHeadID).Select(x => x.EndDate).FirstOrDefault();
                DateTime today = DateTime.Today;

                if (today > endDate)
                {
                    depUser.Role = DepartmentRole.EMPLOYEE;
                    depUser.StartDate = null;
                    depUser.EndDate = null;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("RequisitionsToApprove", "Head");
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult RequisitionsToApprove()
        {
            int headID = headService.FindUserID();
            int depID = headService.FindDepID();
            List<DeptEmployee> depEmployees = headService.ListDepartmentEmployees(depID);
            List<Requisition> requisitionsToApprove = headService.ListRequisitionsByStatus("Pending");
            List<int> empIDs = headService.ListDepEmpsWithPendingRequisitions(depEmployees, requisitionsToApprove);

            //list of requisitions in dep by id
            List<int> reqIDs = new List<int>();

            if (empIDs != null)
            {
                for (int i = 0; i < empIDs.Count; i++)
                {
                    for (int j = 0; j < requisitionsToApprove.Count; j++)
                    {
                        int empID = requisitionsToApprove.Select(x => x.EmployeeID).ElementAt(j);
                        int reqID = requisitionsToApprove.Select(x => x.RequisitionID).ElementAt(j);
                        if (empID == empIDs[i])
                        {
                            reqIDs.Add(reqID);
                        }
                    }
                }

                List<RequisitionDetail> requisitionDetails = db.RequisitionDetails.ToList();

                //list of 1st item in req
                List<string> items = new List<string>();
                //list of 2nd item in req
                List<string> items2 = new List<string>();
                //list of 3rd item in req
                List<string> items3 = new List<string>();

                //list of 1st item quantity
                List<int> quantities = new List<int>();
                //list of 2nd item quantity
                List<int?> quantities2 = new List<int?>();
                //list of 2nd item quantity
                List<int?> quantities3 = new List<int?>();

                List<string> employeeNames = new List<string>();
                List<int> reqIds = new List<int>();
                List<bool> moreThan3 = new List<bool>();
                bool more = false;

                for (int k = 0; k < reqIDs.Count; k++)
                {
                    for (int l = 0; l < requisitionDetails.Count; l++)
                    {
                        int reqID = requisitionDetails.Select(x => x.RequisitionID).ElementAt(l);
                        if (reqID == reqIDs[k])
                        {
                            List<Item> reqItems = headService.ListRequisitionDetailsByReqID(reqID).Select(x => x.Item).ToList();
                            List<int> reqQuantities = headService.ListRequisitionDetailsByReqID(reqID).Select(x => x.Quantity).ToList();
                            int reqEmployeeID = headService.FindRequisitionByReqID(reqID).EmployeeID;
                            string name = headService.FindNameByID(reqEmployeeID);
                            employeeNames.Add(name);
                                if (reqItems.Count == 1)
                                {
                                    string item = reqItems[0].Description;
                                    items.Add(item);
                                    int quantity = reqQuantities[0];
                                    quantities.Add(quantity);
                                    items2.Add(null);
                                    quantities2.Add(null);
                                    items3.Add(null);
                                    quantities3.Add(null);
                                    moreThan3.Add(more);
                                }

                                else if (reqItems.Count == 2)
                                {
                                    string item = reqItems[0].Description;
                                    items.Add(item);
                                    int quantity = reqQuantities[0];
                                    quantities.Add(quantity);
                                    string item2 = reqItems[1].Description;
                                    items2.Add(item2);
                                    int quantity2 = reqQuantities[1];
                                    quantities2.Add(quantity2);
                                    items3.Add(null);
                                    quantities3.Add(null);
                                    moreThan3.Add(more);
                                }

                                else if (reqItems.Count == 3 || reqItems.Count > 3)
                                {
                                    string item = reqItems[0].Description;
                                    items.Add(item);
                                    int quantity = reqQuantities[0];
                                    quantities.Add(quantity);
                                    string item2 = reqItems[1].Description;
                                    items2.Add(item2);
                                    int quantity2 = reqQuantities[1];
                                    quantities2.Add(quantity2);
                                    string item3 = reqItems[2].Description;
                                    items3.Add(item3);
                                    int quantity3 = reqQuantities[2];
                                    quantities3.Add(quantity3);
                                    

                                    if (reqItems.Count > 3)
                                    {
                                        more = true;
                                        
                                    }
                                moreThan3.Add(more);
                            }

                                if (reqItems.Count > 1)
                                {
                                    l = l + (reqItems.Count - 1);
                                }
                            
                        }
                    }
                }

                ViewBag.items = items;
                ViewBag.items2 = items2;
                ViewBag.items3 = items3;
                ViewBag.quantities = quantities;
                ViewBag.quantities2 = quantities2;
                ViewBag.quantities3 = quantities3;
                ViewBag.employees = employeeNames;
                ViewBag.reqIDs = reqIDs;
                ViewBag.more = moreThan3;
            }
            else
            {
                ViewBag.none = "There are no pending requisitions.";
            }
            return View("~/Views/Department/Head/RequisitionsToApprove.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult RequisitionDetails(int RequisitionID, string message)
        {
            List<RequisitionDetail> requisitionDetail = headService.ListRequisitionDetailsByReqID(RequisitionID);
            ViewBag.reqDetail = requisitionDetail;

            List<int> reqItemIDs = requisitionDetail.Select(x => x.ItemID).ToList();
            List<string> reqItemNames = new List<string>();

            for (int i = 0; i < reqItemIDs.Count; i++)
            {
                int reqItemID = reqItemIDs[i];
                string itemName = db.Items.Where(x => x.ItemID == reqItemID).Select(x => x.Description).First().ToString();
                reqItemNames.Add(itemName);
            }

            ViewBag.itemNames = reqItemNames;

            Requisition requisition = headService.FindRequisitionByReqID(RequisitionID);
            ViewBag.req = requisition;

            string date = requisition.RequisitionDate.ToShortDateString();
            ViewBag.date = date;

            int reqEmployeeID = requisition.EmployeeID;
            string name = headService.FindNameByID(reqEmployeeID);
            ViewBag.name = name;

            ViewBag.message = message;

            return View("~/Views/Department/Head/RequisitionDetails.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult Decision(int reqID, string remarks, string button)
        {
            string message = headService.ApproveRejectRequisition(reqID, remarks, button);
            return RedirectToAction("RequisitionDetails", "Head", new { RequisitionID = reqID, message });
        }

        [CustomAuthorize(Roles = "ACTINGHEAD")]
        public ActionResult AssignRep()
        {
            int userID = headService.FindUserID();
            int depID = headService.FindDepID();
            List<DeptEmployee> depEmployees = headService.ListDepartmentEmployees(depID);
            depEmployees = headService.RemoveNonEmployees(depEmployees, depID);

            var model = depEmployees;
            ViewBag.depEmployees = (from item in model select new SelectListItem { Text = item.FirstName + " " + item.LastName, Value = item.DeptEmployeeID.ToString() });

            //current representative
            string repName = headService.FindNameByID(headService.FindRepID(depID));
            ViewBag.representative = repName;

            return View("~/Views/Department/Head/AssignRep.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        [HttpPost]
        public ActionResult SuccessfulAssignRep(FormCollection form, int DeptEmployeeID)
        {
            headService.AssignRep(DeptEmployeeID);

            string newRepName = headService.FindNameByID(DeptEmployeeID);
            ViewBag.newRep = newRepName;
            return View("~/Views/Department/Head/SuccessfulAssignRep.cshtml");
        }

        //assuming head/acting head and rep cannot be the same person
        //assume can only have one acting head at a time.
        [CustomAuthorize(Roles = "HEAD")]
        public ActionResult AssignActingHead()
        {
            int userID = headService.FindUserID();
            int deptID = headService.FindDepID();
            List<DeptEmployee> deptEmployees = headService.ListDepartmentEmployees(deptID);
            deptEmployees = headService.RemoveNonEmployees(deptEmployees, deptID);

            var model = deptEmployees;
            ViewBag.depEmployees = (from item in model select new SelectListItem { Text = item.FirstName + " " + item.LastName, Value = item.DeptEmployeeID.ToString() });

            //if acting head has been assigned, details will be shown.
            int actingHeadID = headService.FindActingHeadID(deptID);

            if (actingHeadID != 0)
            {
                string actingHeadName = headService.FindNameByID(actingHeadID);
                ViewBag.actingHead = actingHeadName;

                string start = headService.FindActingHeadStartDate(actingHeadID);
                string end = headService.FindActingHeadEndDate(actingHeadID);

                ViewBag.startDate = start;
                ViewBag.endDate = end;
            }
            else
                ViewBag.actingHead = "None";

            return View("~/Views/Department/Head/AssignActingHead.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD")]
        [HttpPost]
        public ActionResult SuccessfulAssignActingHead(FormCollection form, int DeptEmployeeID, DateTime start, DateTime end)
        {
            headService.AssignActingHead(DeptEmployeeID, start, end);
            string actingHeadName = headService.FindNameByID(DeptEmployeeID);
            ViewBag.actingHead = actingHeadName;

            string startDate = start.ToString("dd/MM/yyyy");
            string endDate = end.ToString("dd/MM/yyyy");

            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            return View("~/Views/Department/Head/SuccessfulAssignActingHead.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD")]
        public ActionResult CancelActingHead()
        {
            int userID = headService.FindUserID();
            int depID = headService.FindDepID();
            List<DeptEmployee> depEmployees = headService.ListDepartmentEmployees(depID);

            //if acting head has been assigned, details will be shown.
            int actingHeadID = headService.FindActingHeadID(depID);
            ViewBag.actingHeadID = actingHeadID;

            if (actingHeadID != 0)
            {
                string actingHeadName = headService.FindNameByID(actingHeadID);
                ViewBag.actingHead = actingHeadName;

                string start = headService.FindActingHeadStartDate(actingHeadID);
                string end = headService.FindActingHeadEndDate(actingHeadID);

                ViewBag.startDate = start;
                ViewBag.endDate = end;
            }
            else
                ViewBag.actingHead = "There is no current acting head.";

            return View("~/Views/Department/Head/CancelActingHead.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD")]
        [HttpPost]
        public ActionResult SuccessfulCancelActingHead(FormCollection form)
        {
            int depID = headService.FindDepID();
            int actingHeadID = headService.FindActingHeadID(depID);
            headService.CancelActingHead(actingHeadID);
            DeptEmployee depEmployee = db.DeptEmployees.Where(x => x.DeptEmployeeID == actingHeadID).First();
            string name = depEmployee.ToString();
            ViewBag.actingHead = name;
            return View("~/Views/Department/Head/SuccessfulCancelActingHead.cshtml");
        }


        //Author: Phung Khanh Chi
        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult ViewReport()
        {
            var departments = db.Departments.ToList();
            var categories = db.Categories.ToList();
            ViewBag.departments = departments;
            ViewBag.categories = categories;
            return View("~/Views/Department/Head/ViewReport.cshtml");
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        [HttpPost]
        public ActionResult ViewReport(RequisitionReport reqReport)
        {
            Session["report"] = reqReport;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult ShowReport()
        {
            RequisitionReport report = Session["report"] as RequisitionReport;
            if (report.CategoryList == null)
                report.CategoryList = db.Categories.Select(c => c.CategoryId).ToList();

            List<int> departmentList = new List<int>
            {
                headService.FindDepID()
            };
            report.DepartmentList = departmentList;
            ViewBag.report = report;
            reportService.GetRequisitionData(report, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt);
            if (timeDt.Count() == 0 || catDt.Count() == 0)
            {
                ViewBag.message = "Your department made no requisition for these categories during this period";
                return PartialView("~/Views/Shared/_Report.cshtml", null);
            }
            else
            {
                DataTable dt = reportService.GetTable(categories, report.DepartmentList, out int[] catSize, 
                    out string tablePeriod, timeDt, report.StartDate, report.EndDate, REQUISITION);
                ViewBag.catSize = catSize;
                ViewBag.tableName = "Department requisition quantity by category from " + tablePeriod;
                return PartialView("~/Views/Shared/_Report.cshtml", dt);
            }
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public void ExportReqDataTable(string report)
        {
            RequisitionReport reqReport = reportService.GetReqReport(report);
            reportService.GetRequisitionData(reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt);
            DataTable dt = reportService.GetTable(categories, reqReport.DepartmentList, 
                out int[] catSize, out string tablePeriod, timeDt, reqReport.StartDate, reqReport.EndDate, REQUISITION);
            string fileName = "Requisition " + tablePeriod.Replace('/', '-');
            ExportData(dt, fileName, catSize, categories);
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public void ExportData(DataTable dt, string fileName, int[] catSize, string[] categories)
        {
            reportService.ExportData(dt, fileName, catSize, categories);
        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult ShowRequisitionChart(string report, string type, string groupBy)
        {
            MemoryStream imgStream = reportService.ShowRequisitionChart(report, type, groupBy);
            return File(imgStream, "image/png");
        }


        //Author: Ettiyan Premalatha
        //Modified by Feng Li Ying, Phung Khanh Chi
        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        public ActionResult ChangeCollectionPoint()
        {
            int deptID = headService.FindDepID();
            var collectionPoint = (db.Departments.Where(d => d.DepartmentID == deptID).SingleOrDefault()).CollectionPoint;
            var collectionPoints = db.CollectionPoints.Where(c => c.CollectionPointID != collectionPoint.CollectionPointID);
            ViewBag.collectionPointName = collectionPoint.CollectionPointName;
            ViewBag.deptID = deptID;
            ViewBag.deptName = db.Departments.Find(deptID).DepartmentName;
            return View("~/Views/Department/Head/CollectionPoint.cshtml", collectionPoints.ToList());

        }

        [CustomAuthorize(Roles = "HEAD, ACTINGHEAD")]
        [HttpPost]
        public ActionResult ChangeCollectionPoint(string location, string departmentId)
        {
            var locationId = int.Parse(location);
            var departmentid = int.Parse(departmentId);
            var department = db.Departments.Where(x => x.DepartmentID == departmentid).SingleOrDefault();
            department.CollectionPointID = locationId;
            db.SaveChanges();
            var newDp = db.CollectionPoints.Where(
             x => x.CollectionPointID == locationId).SingleOrDefault();
            ViewBag.DepartmentName = department.DepartmentName;
            ViewBag.NewCollectionPoint = newDp.CollectionPointName;
            var dt = DateTime.Today.ToString("dd/MM/yyyy");
            ViewBag.CurrentDate = dt;
            return View("~/Views/Department/Head/ChangeCollectionPoint.cshtml");
        }
    }
}