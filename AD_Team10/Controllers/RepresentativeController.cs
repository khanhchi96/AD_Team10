using AD_Team10.DAL;
using AD_Team10.DataService;
using AD_Team10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers
{
    public class RepresentativeController : Controller
    {
        // GET: Representative
        private RepresentativeService repService = new RepresentativeService();
        private RequisitionService reqService = new RequisitionService();
        DBContext cp;

        //Index and ChangeLocation is coded by Prema. ViewCollectionPoint and ChangeCollectionPoint is done by Wang. Basiclly, it's
        //the same function, can choose anyone's.
        public ActionResult Index(int id)
        {
            //Supposed to use Session to get the department Id
            cp = new DBContext();
            var department = cp.Departments.Where(x => x.DepartmentID == id).SingleOrDefault();
            var collectionPoint = cp.CollectionPoints.Find(department.CollectionPointID);
            ViewBag.CollectionPoint = collectionPoint.CollectionPointName;
            ViewBag.DepartmentName = department.DepartmentName;
            ViewBag.DepartmentID = department.DepartmentID;
            return View();
        }

        public ActionResult ChangeLocation(string location, string departmentId)
        {

            var locationId = int.Parse(location);
            var departmentid = int.Parse(departmentId);
            cp = new DBContext();
            var department = cp.Departments.Where(
              x => x.DepartmentID == departmentid).SingleOrDefault();
            department.CollectionPointID = locationId;
            cp.SaveChanges();
            var newDp = cp.CollectionPoints.Where(
             x => x.CollectionPointID == locationId).SingleOrDefault();
            ViewBag.DepartmentName = department.DepartmentName;
            ViewBag.NewCollectionPoint = newDp.CollectionPointName;
            var dt = DateTime.Today;
            ViewBag.CurrentDate = dt;
            return View();
        }

        public ActionResult ViewCollectionPoint(int id)
        {
            //Supposed to use Session to get the department Id
            Department department = repService.GetDepartmentById(id);
            return View(department);
        }

        public ActionResult ChangeCollectionPoint(int id)
        {
            Department department = repService.GetDepartmentById(id);
            ViewBag.CollectionPointID = new SelectList(repService.GetCollectionPointsList(), "CollectionPointID", "CollectionPointName");
            return View(department);
        }

        [HttpPost]
        public ActionResult ChangeCollectionPoint(Department department)
        {
            ViewBag.CollectionPointID = new SelectList(repService.GetCollectionPointsList(), "CollectionPointID", "CollectionPointName", department.CollectionPointID);
            if (department != null)
                {
                    Department editedDepartment = repService.GetDepartmentById(department.DepartmentID);
                    editedDepartment.CollectionPointID = department.CollectionPointID;
                    repService.UpdateDepartment(editedDepartment);
                    return RedirectToAction("ViewCollectionPoint", new { id = department.DepartmentID });
                }
            else
                return View(department);
        }

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

    }
}