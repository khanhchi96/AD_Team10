using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.Service;

namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "REPRESENTATIVE")]
    public class RepresentativeController : Controller
    {
        private DBContext db = new DBContext();
        private RequisitionService reqService = new RequisitionService();
        public ActionResult Index()
        {
            int deptID = FindDepartmentId();
            var collectionPoint = (db.Departments.Where(d => d.DepartmentID == deptID).SingleOrDefault()).CollectionPoint;

            ViewBag.collectionPointName = collectionPoint.CollectionPointName;
            ViewBag.deptID = deptID;
            ViewBag.deptName = db.Departments.Find(deptID).DepartmentName;
            return View("~/Views/Department/Representative/Index.cshtml");

        }

        private int FindDepartmentId()
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int employeeId = user.UserID;
            int deptID = (db.DeptEmployees.Where(dE => dE.DeptEmployeeID == employeeId).SingleOrDefault()).DepartmentID;
            return deptID;
        }
        public ActionResult ChangeLocation(string location, string departmentId)
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
            var dt = DateTime.Today;
            ViewBag.CurrentDate = dt;
            return View("~/Views/Department/Representative/ChangeLocation.cshtml");
        }


        public ActionResult EmployeeRequisitions()
        {
            int deptId = FindDepartmentId();
            var requisitions = db.Requisitions.Where(r => r.Employee.DepartmentID == deptId &&
                            (r.Status == Status.Approved || r.Status == Status.Incomplete || r.Status == Status.Completed));
            return View("~/Views/Department/Representative/EmployeeRequisitions.cshtml", requisitions.ToList());
        }
       

        public ActionResult EmployeeRequisitionDetails(int id)
        {
            var details = db.RequisitionDetails.Where(r => r.RequisitionID == id);
            return View("~/Views/Department/Representative/EmployeeRequisitionDetails.cshtml", details.ToList());
        }

        public ActionResult UpdateEmployeeRequisition(int id)
        {
            Requisition requisition = reqService.GetRequisitionById(id);
            return View("~/Views/Department/Representative/UpdateEmployeeRequisition.cshtml", requisition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEmployeeRequisition(Requisition requisition)
        {
            if (ModelState.IsValid)
            {
                Requisition editedRequisition = reqService.GetRequisitionById(requisition.RequisitionID);
                List<RequisitionDetail> editedDetails = editedRequisition.RequisitionDetails;
                List<RequisitionDetail> details = requisition.RequisitionDetails;
                for (int i = 0; i < editedRequisition.RequisitionDetails.Count; i++)
                {
                    details[i].QuantityReceived = details[i].QuantityReceived;
                }
                
                if (reqService.IsCompleted(editedRequisition))
                {
                    editedRequisition.CompletedDate = DateTime.Now;
                    editedRequisition.Status = Status.Completed;
                }
                else
                {
                    editedRequisition.Status = Status.Incomplete;
                    RetrievalList retrievalList = reqService.FindCurrentRetrievalList();
                    if (retrievalList == null) retrievalList = reqService.CreateRetrievalList();
                    reqService.IncompletedRequisitionTransferToRetrieval(editedRequisition, retrievalList);
                    reqService.UpdateRetrievalList(retrievalList);
                }

                db.SaveChanges();
                return RedirectToAction("EmployeeRequisitionDetails", new { id = requisition.RequisitionID });
            }
            return View(requisition);
        }

        // GET: Pending requisition list
        public ActionResult GetPendingList()
        {
            var requisitions = reqService.GetPendingRequisitions();
            var department = new Models.Department();
            var collectionPoint = new CollectionPoint();
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName");
            ViewBag.CollectionPointID = new SelectList(reqService.GetCollectionPoints(), "CollectionPointID", "CollectionPointName");
            return View(Tuple.Create(requisitions, department, collectionPoint));
        }

        [HttpPost]
        public ActionResult GetPendingList(Models.Department department, CollectionPoint collectionPoint)
        {
            var requisitions = reqService.GetPendingRequisitions();
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
                            return View("~/Views/Department/Representative/GetPendingList.cshtml", Tuple.Create(requisitions.ToList(), new Models.Department(), new CollectionPoint()));
                        }
                        else
                        {
                            requisitions = requisitions.Where(x => x.Employee.Department.CollectionPointID == collectionPoint.CollectionPointID).ToList();
                            return View("~/Views/Department/Representative/GetPendingList.cshtml", Tuple.Create(requisitions, department, collectionPoint));
                        }
                    }
                    else
                    {
                        return View("~/Views/Department/Representative/GetPendingList.cshtml", Tuple.Create(requisitions.ToList(), new Models.Department(), new CollectionPoint()));
                    }
                }
                else
                {
                    requisitions = requisitions.Where(x => x.Employee.DepartmentID == department.DepartmentID).ToList();
                    return View("~/Views/Department/Representative/GetPendingList.cshtml", Tuple.Create(requisitions, department, collectionPoint));
                }
            }
            else
            {
                return View("~/Views/Department/Representative/GetPendingList.cshtml", Tuple.Create(requisitions, department, collectionPoint));
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

        

    





    }
}