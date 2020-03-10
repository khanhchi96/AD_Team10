using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.Service;
using PagedList;

//Author: Phung Khanh Chi, Wang Wang Wang
namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "REPRESENTATIVE")]
    public class RepresentativeController : Controller
    {
        static readonly int PAGE_SIZE = 8;
        private DBContext db = new DBContext();
        private RequisitionService reqService = new RequisitionService();
        private RepresentativeService repService = new RepresentativeService();
        private EmployeeController employeeController = new EmployeeController();
        private EmployeeService employeeService = new EmployeeService();


        //Author: Phung Khanh Chi
        public ActionResult Index()
        {
            List<Requisition> employeeRequisitions = repService.GetEmployeeRequisition()
                                            .Take(PAGE_SIZE).ToList();
            int retrievalListId = reqService.FindCurrentRetrievalList().RetrievalListID - 1;
            List<RetrievalListDetail> disbursementList = repService.GetDisbursementList(retrievalListId);
            List<Requisition> myRequisitions = employeeService.GetRequisitions(); 

            ViewBag.empRequisitions = employeeRequisitions;
            ViewBag.disbursementList = disbursementList;
            ViewBag.myRequisitions = myRequisitions;
            return View("~/Views/Department/Representative/Index.cshtml");
        }

        public ActionResult MyRequisitions(int? page, string status = "")
        {
            return employeeController.Index(page, status);
        }

        [HttpGet]
        public ActionResult MyRequisitionDetails(int requisitionId)
        {
            return employeeController.RequisitionDetails(requisitionId);           
        }


        [HttpGet]
        public ActionResult UpdateMyRequisition(int requisitionId)
        {
            return employeeController.UpdateRequisition(requisitionId);
        }

        [HttpPost]
        public ActionResult UpdateAndSave(int requisitionId, FormCollection form)
        {
            employeeService.UpdateAndSave(requisitionId, form);
            return RedirectToAction("MyRequisitionDetails", new { requisitionId });
        }

        public ActionResult CreateMyRequisition()
        {
            return employeeController.CreateRequisition();
        }

        [HttpPost]
        public ActionResult CreateMyRequisition(Requisition requisition, FormCollection form)
        {
            employeeService.CreateRequisition(requisition, form);
            return RedirectToAction("MyRequisitions");
        }

        [HttpGet]
        public ActionResult DeleteMyRequisition(int requisitionId)
        {
            employeeService.DeleteRequisition(requisitionId);
            return RedirectToAction("MyRequisitions");
        }

        public ActionResult DisbursementList()
        {
            RetrievalList retrievalList = reqService.FindCurrentRetrievalList();
            int retrievalListId = retrievalList.RetrievalListID - 1;
            List<RetrievalListDetail> disbursementList = repService.GetDisbursementList(retrievalListId);
            RetrievalList lastRetrievalList = db.RetrievalLists.SingleOrDefault(r => r.RetrievalListID == retrievalListId);
            ViewBag.startDate = lastRetrievalList.StartDate.ToString("dd/MM/yyyy");
            ViewBag.endDate = lastRetrievalList.EndDate.ToString("dd/MM/yyyy");
            return View("~/Views/Department/Representative/DisbursementList.cshtml", disbursementList);
        }


        public ActionResult EmployeeRequisitions(int? page, string status="")
        {
            ViewBag.statusList = new List<string> { Status.Approved.ToString(), Status.Incomplete.ToString(), Status.Completed.ToString()};
            List<Requisition> requisitions = repService.GetEmployeeRequisition();
            if (status != "") { requisitions = requisitions.Where(r => r.Status.ToString() == status).ToList(); }
            int pageNumber = (page ?? 1);
            return View("~/Views/Department/Representative/EmployeeRequisitions.cshtml", requisitions.ToPagedList(pageNumber, PAGE_SIZE));
        }
       

        public ActionResult EmployeeRequisitionDetails(int id)
        {
            var requisition = db.Requisitions.SingleOrDefault(r => r.RequisitionID == id);
            return View("~/Views/Department/Representative/EmployeeRequisitionDetails.cshtml", requisition);
        }


        //Author: Wang Wang Wang
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
                    editedRequisition.Status = Status.Incomplete;
                    RetrievalList retrievalList = reqService.FindCurrentRetrievalList();
                    if (retrievalList == null) retrievalList = reqService.CreateRetrievalList();
                    reqService.IncompletedRequisitionTransferToRetrieval(editedRequisition, retrievalList);
                    reqService.UpdateRetrievalList(retrievalList);
                }
                reqService.UpdateRequisition(editedRequisition);
                return RedirectToAction("EmployeeRequisitionDetails", new { id = requisition.RequisitionID });
            }
            return View("~/Views/Department/Representative/UpdateEmployeeRequisition.cshtml", requisition);
        }

    }
}