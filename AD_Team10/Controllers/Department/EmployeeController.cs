using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;
using PagedList;
using AD_Team10.Service;

//Author: Feng Li Ying
namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        static readonly int PAGE_SIZE = 8;
        DBContext db = new DBContext();
        private EmployeeService employeeService = new EmployeeService();
        public ActionResult Index(int? page, string status = "")
        {
            ViewBag.statusList = new List<string> { Status.Pending.ToString(), Status.Approved.ToString(),
                    Status.Rejected.ToString(), Status.Incomplete.ToString(), Status.Completed.ToString()};
            var requisitions = employeeService.GetRequisitions();
            if (status != "") { requisitions = requisitions.Where(r => r.Status.ToString() == status).ToList(); }
            ViewBag.status = status;
            ViewBag.Requisitions = requisitions;
            ViewBag.employeeID = employeeService.GetEmployeeID();
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
            employeeService.UpdateAndSave(requisitionId, form);       
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
            employeeService.CreateRequisition(requisition, form);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteRequisition(int requisitionId)
        {
            employeeService.DeleteRequisition(requisitionId);
            return RedirectToAction("Index");
        }
    }
}