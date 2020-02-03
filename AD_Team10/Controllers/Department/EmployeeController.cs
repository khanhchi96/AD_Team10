using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AD_Team10.DAL;
using AD_Team10.Models;

namespace AD_Team10.Controllers.Department
{
    [Authorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        DBContext db = new DBContext();
        public ActionResult Index(int id)
        {
            var query = (from rq in db.Requisitions
                             where rq.EmployeeID == id
                             select rq).ToList();
            ViewBag.Rqs = query;
            ViewBag.employeeID = id;
            return View("~/Views/Department/Employee/Index.cshtml");
        }

        [HttpGet]
        public ActionResult rqDetails(int requisitionId)
        {
            var query = (from rqDetail in db.RequisitionDetails
                         where rqDetail.RequisitionID == requisitionId
                         select rqDetail).ToList();
            var Requisition = db.Requisitions.Find(requisitionId);

            ViewBag.rqDetails = query;
            ViewBag.rqID = requisitionId;
            ViewBag.Requisition = Requisition;
            ViewBag.employeeID = Requisition.EmployeeID;
            return View("~/Views/Department/Employee/rqDetails.cshtml");
        }

        [HttpGet]
        public ActionResult updateRequisition(int requisitionId)
        {
            var query = (from rqDetail in db.RequisitionDetails
                         where rqDetail.RequisitionID == requisitionId
                         select rqDetail).ToList();
            var items = (from item in db.Items
                         select item).ToList();

            ViewBag.rqDetails = query;
            ViewBag.items = items;
            ViewBag.rqID = requisitionId;
            return View("~/Views/Department/Employee/updateRequisition.cshtml",query);

        }
        [HttpPost]
        public ActionResult updateAndSave(int requisitionId, FormCollection form)
        {

            var rds = (from rqDetail in db.RequisitionDetails
                      where rqDetail.RequisitionID == requisitionId
                      select rqDetail).ToList();
            
            foreach( var rd in rds)
            {
                db.RequisitionDetails.Remove(rd);
                db.SaveChanges();
            }

            var Items = form.GetValues("Item");
            var Quantities = form.GetValues("Quantity");
            
            
            for(int i=0; i < Items.Count(); i++)
            {
                RequisitionDetail newReqDetail = new RequisitionDetail();
                newReqDetail.RequisitionID = requisitionId;
                newReqDetail.ItemID = Int32.Parse(Items[i]);
                newReqDetail.Quantity = Int32.Parse(Quantities[i]);
                newReqDetail.QuantityReceived = 0;

                db.RequisitionDetails.Add(newReqDetail);
                db.SaveChanges();

            }

            return RedirectToAction("rqDetails", new { requisitionId=requisitionId});
        }


        public ActionResult CreateRequisition(int employeeID)
        {
            DepEmployee Employee = db.DepEmployees.Find(employeeID);
            var query = (from item in db.Items
                         select item).ToList();
            ViewBag.employee = Employee;
            ViewBag.items = query;
            ViewBag.employeeID = employeeID;
            return View("~/Views/Department/Employee/createRequisition.cshtml");
        }

        [HttpPost]
        public ActionResult CreateRequisition(Requisition requisition, FormCollection form)
        {
            Requisition newReq = new Requisition();
            newReq.RequisitionDate = requisition.RequisitionDate;
            newReq.Status = requisition.Status;
            newReq.EmployeeID = requisition.EmployeeID;

            db.Requisitions.Add(newReq);

            var ItemDescriptions = form.GetValues("ItemDescription");
            var ItemQuantitys = form.GetValues("ItemQuantity");

            for (int i = 0; i < ItemDescriptions.Count(); i++)
            {
                var temp = ItemDescriptions[i];

                    int itemID = (from item in db.Items
                                  where item.Description == temp
                                  select item.ItemID).FirstOrDefault();


                    RequisitionDetail newReqDetail = new RequisitionDetail();
                    newReqDetail.RequisitionID = newReq.RequisitionID;
                    newReqDetail.ItemID = itemID;
                    newReqDetail.Quantity = Int32.Parse(ItemQuantitys[i]);
                    newReqDetail.QuantityReceived = 0;

                    db.RequisitionDetails.Add(newReqDetail);
                    db.SaveChanges();  
            }

            return RedirectToAction("Index",new { id=newReq.EmployeeID});
        }

        [HttpGet]
        public ActionResult DeleteRequisition(int requisitionId)
        {
            var Requisition = db.Requisitions.Find(requisitionId);
            db.Requisitions.Remove(Requisition);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=Requisition.EmployeeID});
        }

    }

}