using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;
using PagedList;

namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        static readonly int PAGE_SIZE = 8;
        DBContext db = new DBContext();
        public ActionResult Index(int? page, string status = "")
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int employeeId = user.UserID;
            ViewBag.statusList = new List<string> { Status.Pending.ToString(), Status.Approved.ToString(),
                    Status.Rejected.ToString(), Status.Incomplete.ToString(), Status.Completed.ToString()};
            var requisitions = db.Requisitions.Where(r => r.EmployeeID == employeeId).OrderByDescending(r => r.RequisitionDate).ToList();
            if (status != "") { requisitions = requisitions.Where(r => r.Status.ToString() == status).ToList(); }
            ViewBag.status = status;
            ViewBag.Requisitions = requisitions;
            ViewBag.employeeID = employeeId;
            int pageNumber = (page ?? 1);
            return View("~/Views/Department/Employee/Index.cshtml", requisitions.ToPagedList(pageNumber, PAGE_SIZE));
        }

        [HttpGet]
        public ActionResult RequisitionDetails(int requisitionId)
        {
            var query = (from rqDetail in db.RequisitionDetails
                         where rqDetail.RequisitionID == requisitionId
                         select rqDetail).ToList();
            var Requisition = db.Requisitions.Find(requisitionId);

            ViewBag.rqDetails = query;
            ViewBag.rqID = requisitionId;
            ViewBag.Requisition = Requisition;
            ViewBag.employeeID = Requisition.EmployeeID;
            return View("~/Views/Department/Employee/RequisitionDetails.cshtml");
        }


        [HttpGet]
        public ActionResult UpdateRequisition(int requisitionId)
        {
            var query = (from rqDetail in db.RequisitionDetails
                         where rqDetail.RequisitionID == requisitionId
                         select rqDetail).ToList();
            var items = (from item in db.Items
                         select item).ToList();

            ViewBag.rqDetails = query;
            ViewBag.items = items;
            ViewBag.rqID = requisitionId;
            return View("~/Views/Department/Employee/UpdateRequisition.cshtml", query);

        }

        [HttpPost]
        public ActionResult UpdateAndSave(int requisitionId, FormCollection form)
        {

            var rds = (from rqDetail in db.RequisitionDetails
                       where rqDetail.RequisitionID == requisitionId
                       select rqDetail).ToList();

            foreach (var rd in rds)
            {
                db.RequisitionDetails.Remove(rd);
                db.SaveChanges();
            }

            var Items = form.GetValues("Item");
            var Quantities = form.GetValues("Quantity");

            for (int i = 0; i < Items.Count(); i++)
            {
                RequisitionDetail newReqDetail = new RequisitionDetail
                {
                    RequisitionID = requisitionId,
                    ItemID = Int32.Parse(Items[i]),
                    Quantity = Int32.Parse(Quantities[i]),
                    QuantityReceived = 0
                };


                db.RequisitionDetails.Add(newReqDetail);
                db.SaveChanges();

            }

            return RedirectToAction("RequisitionDetails", new { requisitionId });
        }

        public ActionResult CreateRequisition()
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int employeeId = user.UserID;
            DeptEmployee Employee = db.DeptEmployees.SingleOrDefault(d => d.DeptEmployeeID == employeeId);
            var query = (from item in db.Items
                         select item).ToList();
            ViewBag.employee = Employee;
            ViewBag.items = query;
            ViewBag.employeeID = employeeId;
            return View("~/Views/Department/Employee/CreateRequisition.cshtml");
        }

        [HttpPost]
        public ActionResult CreateRequisition(Requisition requisition, FormCollection form)
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int departmentId = db.DeptUsers.SingleOrDefault(d => d.DeptUserID == user.UserID).DeptEmployee.DepartmentID;

            /*
            RetrievalList retrievalList = db.RetrievalLists.SingleOrDefault(r => r.StartDate <= DateTime.Today && r.EndDate >= DateTime.Today);
            if (retrievalList == null)
            {
                DateTime today = DateTime.Today;
                int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
                DateTime nextFriday = today.AddDays(daysUntilFriday);
                DateTime lastSaturday = nextFriday.AddDays(-6);
                retrievalList = new RetrievalList { StartDate = lastSaturday, EndDate = nextFriday };
                db.RetrievalLists.Add(retrievalList);
                db.SaveChanges();
            }
            int retrievalListId = retrievalList.RetrievalListID;
            */
            Requisition newReq = new Requisition
            {
                RequisitionDate = requisition.RequisitionDate,
                Status = requisition.Status,
                EmployeeID = user.UserID
            };
            //newReq.RetrievalListID = retrievalListId;

            db.Requisitions.Add(newReq);

            var ItemDescriptions = form.GetValues("ItemDescription");
            var ItemQuantitys = form.GetValues("ItemQuantity");

            for (int i = 0; i < ItemDescriptions.Count(); i++)
            {
                var temp = ItemDescriptions[i];

                int itemID = (from item in db.Items
                              where item.Description == temp
                              select item.ItemID).FirstOrDefault();


                RequisitionDetail newReqDetail = new RequisitionDetail
                {
                    RequisitionID = newReq.RequisitionID,
                    ItemID = itemID,
                    Quantity = Int32.Parse(ItemQuantitys[i]),
                    QuantityReceived = 0
                };

                db.RequisitionDetails.Add(newReqDetail);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteRequisition(int requisitionId)
        {
            var Requisition = db.Requisitions.Find(requisitionId);
            db.Requisitions.Remove(Requisition);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}