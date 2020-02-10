using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;

namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "HEAD")]
    public class HeadController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            string username = FindUserByUsername();
            int headID = FindUserID(username);
            int depID = FindDepID(headID);
            int actingHeadID = FindActingHeadID(depID);

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

        public ActionResult RequisitionsToApprove()
        {
            string username = FindUserByUsername();
            int headID = FindUserID(username);
            int depID = FindDepID(headID);
            List<DeptEmployee> depEmployees = ListDepartmentEmployees(depID);
            List<Requisition> requisitionsToApprove = ListRequisitionsByStatus("Pending");
            List<int> empIDs = ListDepEmpsWithPendingRequisitions(depEmployees, requisitionsToApprove);

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
                            List<Item> reqItems = ListRequisitionDetailsByReqID(reqID).Select(x => x.Item).ToList();
                            List<int> reqQuantities = ListRequisitionDetailsByReqID(reqID).Select(x => x.Quantity).ToList();
                            int reqEmployeeID = FindRequisitionByReqID(reqID).EmployeeID;
                            string name = FindNameByID(reqEmployeeID);

                            employeeNames.Add(name);

                            for (int m = 0; m < 1; m++)
                            {
                                if (reqItems.Count == 1)
                                {
                                    string item = reqItems[m].Description;
                                    items.Add(item);
                                    int quantity = reqQuantities[m];
                                    quantities.Add(quantity);
                                    items2.Add(null);
                                    quantities2.Add(null);
                                    items3.Add(null);
                                    quantities3.Add(null);
                                    moreThan3.Add(more);
                                }

                                else if (reqItems.Count == 2)
                                {
                                    string item = reqItems[m].Description;
                                    items.Add(item);
                                    int quantity = reqQuantities[m];
                                    quantities.Add(quantity);
                                    string item2 = reqItems[m + 1].Description;
                                    items2.Add(item2);
                                    int quantity2 = reqQuantities[m + 1];
                                    quantities2.Add(quantity2);
                                    items3.Add(null);
                                    quantities3.Add(null);
                                    moreThan3.Add(more);
                                }

                                else if (reqItems.Count == 3 || reqItems.Count > 3)
                                {
                                    string item = reqItems[m].Description;
                                    items.Add(item);
                                    int quantity = reqQuantities[m];
                                    quantities.Add(quantity);
                                    string item2 = reqItems[m + 1].Description;
                                    items2.Add(item2);
                                    int quantity2 = reqQuantities[m + 1];
                                    quantities2.Add(quantity2);
                                    string item3 = reqItems[m + 2].Description;
                                    items3.Add(item3);
                                    int quantity3 = reqQuantities[m + 2];
                                    quantities3.Add(quantity3);
                                    moreThan3.Add(more);

                                    if (reqItems.Count > 3)
                                    {
                                        more = true;
                                        moreThan3.Add(more);
                                    }
                                }

                                if (reqItems.Count > 1)
                                {
                                    l = l + (reqItems.Count - 1);
                                }
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

        public ActionResult RequisitionDetails(int RequisitionID, string message)
        {
            List<RequisitionDetail> requisitionDetail = ListRequisitionDetailsByReqID(RequisitionID);
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

            Requisition requisition = FindRequisitionByReqID(RequisitionID);
            ViewBag.req = requisition;

            string date = requisition.RequisitionDate.ToShortDateString();
            ViewBag.date = date;

            int reqEmployeeID = requisition.EmployeeID;
            string name = FindNameByID(reqEmployeeID);
            ViewBag.name = name;

            ViewBag.message = message;

            return View("~/Views/Department/Head/RequisitionDetails.cshtml");
        }

        public ActionResult Decision(int reqID, string remarks, string button)
        {
            Requisition requisition = FindRequisitionByReqID(reqID);

            string message = null;

            if (remarks != null)
            {
                requisition.Remark = remarks;
            }

            if (button == "Approve")
            {
                requisition.Status = Status.Approved;
                message = "The requisition has been approved.";
            }
            else if (button == "Reject")
            {
                requisition.Status = Status.Rejected;
                message = "The requisition has been rejected.";
            }

            db.SaveChanges();

            return RedirectToAction("RequisitionDetails", "Head", new { RequisitionID = reqID, message });
        }

        public ActionResult AssignRep()
        {
            string username = FindUserByUsername();
            int userID = FindUserID(username);
            int depID = FindDepID(userID);
            List<DeptEmployee> depEmployees = ListDepartmentEmployees(depID);
            depEmployees = RemoveNonEmployees(depEmployees, depID);

            var model = depEmployees;
            ViewBag.depEmployees = (from item in model select new SelectListItem { Text = item.FirstName + " " + item.LastName, Value = item.DeptEmployeeID.ToString() });

            //current representative
            string repName = FindNameByID(FindRepID(depID));
            ViewBag.representative = repName;

            return View("~/Views/Department/Head/AssignRep.cshtml");
        }

        [HttpPost]
        public ActionResult SuccessfulAssignRep(FormCollection form, int DeptEmployeeID)
        {
            int depID = FindDepID(DeptEmployeeID);

            //change current rep to emp
            int repID = FindRepID(depID);
            DeptUser currentRep = FindDepUser(repID);
            currentRep.Role = DepartmentRole.EMPLOYEE;

            //assign selected emp as rep
            DeptUser newRep = FindDepUser(DeptEmployeeID);
            newRep.Role = DepartmentRole.REPRESENTATIVE;

            db.SaveChanges();

            string newRepName = FindNameByID(DeptEmployeeID);
            ViewBag.newRep = newRepName;
            return View("~/Views/Department/Head/SuccessfulAssignRep.cshtml");
        }

        //assuming head/acting head and rep cannot be the same person
        //assume can only have one acting head at a time.
        public ActionResult AssignActingHead()
        {
            string username = FindUserByUsername();
            int userID = FindUserID(username);
            int deptID = FindDepID(userID);
            List<DeptEmployee> deptEmployees = ListDepartmentEmployees(deptID);
            deptEmployees = RemoveNonEmployees(deptEmployees, deptID);

            var model = deptEmployees;
            ViewBag.depEmployees = (from item in model select new SelectListItem { Text = item.FirstName + " " + item.LastName, Value = item.DeptEmployeeID.ToString() });

            //if acting head has been assigned, details will be shown.
            int actingHeadID = FindActingHeadID(deptID);

            if (actingHeadID != 0)
            {
                string actingHeadName = FindNameByID(actingHeadID);
                ViewBag.actingHead = actingHeadName;

                string start = FindActingHeadStartDate(actingHeadID);
                string end = FindActingHeadEndDate(actingHeadID);

                ViewBag.startDate = start;
                ViewBag.endDate = end;
            }
            else
                ViewBag.actingHead = "None";

            return View("~/Views/Department/Head/AssignActingHead.cshtml");
        }


        [HttpPost]
        public ActionResult SuccessfulAssignActingHead(FormCollection form, int DeptEmployeeID, DateTime start, DateTime end)
        {
            DeptUser depUser = FindDepUser(DeptEmployeeID);
            depUser.StartDate = start;
            depUser.EndDate = end;
            DateTime today = DateTime.Today;

            if (start == today)
            {
                depUser.Role = DepartmentRole.ACTINGHEAD;
            }

            db.SaveChanges();

            string actingHeadName = FindNameByID(DeptEmployeeID);
            ViewBag.actingHead = actingHeadName;

            string startDate = start.ToString("dd/MM/yyyy");
            string endDate = end.ToString("dd/MM/yyyy");

            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            return View("~/Views/Department/Head/SuccessfulAssignActingHead.cshtml");
        }

        public ActionResult CancelActingHead()
        {
            string username = FindUserByUsername();
            int userID = FindUserID(username);
            int depID = FindDepID(userID);
            List<DeptEmployee> depEmployees = ListDepartmentEmployees(depID);

            //if acting head has been assigned, details will be shown.
            int actingHeadID = FindActingHeadID(depID);
            ViewBag.actingHeadID = actingHeadID;

            if (actingHeadID != 0)
            {
                string actingHeadName = FindNameByID(actingHeadID);
                ViewBag.actingHead = actingHeadName;

                string start = FindActingHeadStartDate(actingHeadID);
                string end = FindActingHeadEndDate(actingHeadID);

                ViewBag.startDate = start;
                ViewBag.endDate = end;
            }
            else
                ViewBag.actingHead = "There is no current acting head.";

            return View("~/Views/Department/Head/CancelActingHead.cshtml");
        }


        [HttpPost]
        public ActionResult SuccessfulCancelActingHead(FormCollection form)
        {
            string username = FindUserByUsername();
            int headID = FindUserID(username);
            int depID = FindDepID(headID);
            int actingHeadID = FindActingHeadID(depID);

            DeptUser depUser = FindDepUser(actingHeadID);
            depUser.Role = DepartmentRole.EMPLOYEE;
            depUser.StartDate = null;
            depUser.EndDate = null;

            db.SaveChanges();

            DeptEmployee depEmployee = db.DeptEmployees.Where(x => x.DeptEmployeeID == actingHeadID).First();
            string name = depEmployee.FirstName + " " + depEmployee.LastName;
            ViewBag.actingHead = name;
            return View("~/Views/Department/Head/SuccessfulCancelActingHead.cshtml");
        }

        public string FindUserByUsername()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        public List<DeptEmployee> ListDepartmentEmployees(int depID)
        {
            return db.DeptEmployees.Where(x => x.DepartmentID == depID).ToList();
        }

        public int FindUserID(string username)
        {
            return db.DeptUsers.Where(x => x.Username == username).Select(x => x.DeptEmployeeID).First();
        }

        public int FindDepID(int userID)
        {
            return db.DeptEmployees.Where(x => x.DeptEmployeeID == userID).Select(x => x.DepartmentID).First();
        }

        public int FindHeadID(int depID)
        {
            var query = from emp in ListDepartmentEmployees(depID) join user in db.DeptUsers on emp.DeptEmployeeID equals user.DeptEmployeeID where emp.DepartmentID == depID && user.Role.ToString() == "HEAD" select emp.DeptEmployeeID;
            return query.First();
        }

        public int FindRepID(int depID)
        {
            var query = from emp in ListDepartmentEmployees(depID) join user in db.DeptUsers on emp.DeptEmployeeID equals user.DeptEmployeeID where emp.DepartmentID == depID && user.Role.ToString() == "REPRESENTATIVE" select emp.DeptEmployeeID;
            return query.First();
        }

        public int FindActingHeadID(int depID)
        {
            var query = from emp in ListDepartmentEmployees(depID) join user in db.DeptUsers on emp.DeptEmployeeID equals user.DeptEmployeeID where emp.DepartmentID == depID && user.Role.ToString() == "ACTINGHEAD" select emp.DeptEmployeeID;
            return query.FirstOrDefault();
        }

        public List<DeptEmployee> RemoveNonEmployees(List<DeptEmployee> depEmployees, int depID)
        {
            int headID = FindHeadID(depID);
            //remove head from list
            depEmployees = depEmployees.Where(x => x.DeptEmployeeID != headID).ToList();

            int repID = FindRepID(depID);
            //remove rep from list
            depEmployees = depEmployees.Where(x => x.DeptEmployeeID != repID).ToList();

            //if acting head exists, remove acting head from list
            int actingHeadID = FindActingHeadID(depID);
            if (actingHeadID != 0)
            {
                depEmployees = depEmployees.Where(x => x.DeptEmployeeID != actingHeadID).ToList();
            }

            return depEmployees;
        }

        public string FindNameByID(int userID)
        {
            return db.DeptEmployees.Where(x => x.DeptEmployeeID == userID).Select(x => x.FirstName + " " + x.LastName).First().ToString();
        }

        public string FindActingHeadStartDate(int actingHeadID)
        {
            var startDate = db.DeptUsers.Where(x => x.DeptUserID == actingHeadID).Select(x => x.StartDate).First();
            return startDate.HasValue ? startDate.Value.ToString("dd/MM/yyyy") : "";
        }

        public string FindActingHeadEndDate(int actingHeadID)
        {
            var endDate = db.DeptUsers.Where(x => x.DeptUserID == actingHeadID).Select(x => x.EndDate).First();
            return endDate.HasValue ? endDate.Value.ToString("dd/MM/yyyy") : "";
        }

        public DeptUser FindDepUser(int userID)
        {
            return db.DeptUsers.Where(x => x.DeptUserID == userID).First();

        }

        public List<Requisition> ListRequisitionsByStatus(string status)
        {
            return db.Requisitions.Where(x => x.Status.ToString() == status).ToList();
        }

        public List<int> ListDepEmpsWithPendingRequisitions(List<DeptEmployee> depEmployees, List<Requisition> requisitionsToApprove)
        {
            return depEmployees.Select(x => x.DeptEmployeeID).Intersect(requisitionsToApprove.Select(x => x.EmployeeID)).ToList();
        }

        public List<RequisitionDetail> ListRequisitionDetailsByReqID(int reqID)
        {
            return db.RequisitionDetails.Where(x => x.RequisitionID == reqID).ToList();
        }

        public Requisition FindRequisitionByReqID(int reqID)
        {
            return db.Requisitions.Where(x => x.RequisitionID == reqID).First();
        }




    }
}