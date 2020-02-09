using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.Models;
using AD_Team10.DAL;
using AD_Team10.DataService;



namespace AD_Team10.Controllers.Department
{

    [CustomAuthorize(Roles = "REPRESENTATIVE")]
    public class RepresentativeController : Controller
    {
        /*  WWW added two Service models in a file called DataService
         * 
        private RepresentativeService repService = new RepresentativeService();
        private RequisitionService reqService = new RequisitionService();
        */
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int employeeId = user.UserID;
            var deptID = (db.DepEmployees.Where(dE => dE.DepEmployeeID == employeeId).SingleOrDefault()).DepartmentID;
            var collectionPoint = (db.Departments.Where(d => d.DepartmentID == deptID).SingleOrDefault()).CollectionPoint;

            ViewBag.collectionPointName = collectionPoint.CollectionPointName;
            ViewBag.deptID = deptID;
            ViewBag.deptName = db.Departments.Find(deptID).DepartmentName;
            return View("~/Views/Department/Representative/Index.cshtml");

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

        /* conflicts with WWW's following code cuz WWW still got RetrievalList in Requisition model

        // GET: Pending requisition list
        public ActionResult GetPendingList()
        {
            var requisitions = reqService.GetPendingRequisitions();
            var department = new Department();
            var collectionPoint = new CollectionPoint();
            ViewBag.DepartmentID = new SelectList(reqService.GetDepartments(), "DepartmentID", "DepartmentName");
            ViewBag.CollectionPointID = new SelectList(reqService.GetCollectionPoints(), "CollectionPointID", "CollectionPointName");
            return View(Tuple.Create(requisitions, department, collectionPoint));
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

    */





    }
}