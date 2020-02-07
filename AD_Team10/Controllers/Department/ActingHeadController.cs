using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Models;
using AD_Team10.DAL;

namespace AD_Team10.Controllers.Department
{
    [Authorize(Roles = "ACTINGHEAD, EMPLOYEE")]
    public class ActingHeadController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
                string username = System.Web.HttpContext.Current.User.Identity.Name;
                DepUser depUser = db.DepUsers.Where(x => x.Username == username).First();
                DepartmentRole currentRole = db.DepUsers.Where(x => x.Username == username).Select(x => x.Role).First();

                DateTime? endDate = db.DepUsers.Where(x => x.Username == username).Select(x => x.EndDate).FirstOrDefault();
                DateTime today = DateTime.Today;

                if (today > endDate)
                {
                    depUser.Role = DepartmentRole.EMPLOYEE;
                    depUser.StartDate = null;
                    depUser.EndDate = null;
                    db.SaveChanges();
                    currentRole = db.DepUsers.Where(x => x.Username == username).Select(x => x.Role).First();
                    return RedirectToAction("Index", "Employee");
                }

                ViewBag.role = currentRole;

            return View("~/Views/Department/Employee/Index.cshtml");
        }

        public ActionResult RequisitionsToApprove()
        {
            string username = FindUserByUsername();
            int headID = FindUserID(username);
            int depID = FindDepID(headID);
            List<DepEmployee> depEmployees = ListDepartmentEmployees(depID);
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

        public string FindUserByUsername()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        public List<DepEmployee> ListDepartmentEmployees(int depID)
        {
            return db.DepEmployees.Where(x => x.DepartmentID == depID).ToList();
        }

        public int FindUserID(string username)
        {
            return db.DepUsers.Where(x => x.Username == username).Select(x => x.DepEmployeeID).First();
        }

        public int FindDepID(int userID)
        {
            return db.DepEmployees.Where(x => x.DepEmployeeID == userID).Select(x => x.DepartmentID).First();
        }

        public List<Requisition> ListRequisitionsByStatus(string status)
        {
            return db.Requisitions.Where(x => x.Status.ToString() == status).ToList();
        }

        public List<int> ListDepEmpsWithPendingRequisitions(List<DepEmployee> depEmployees, List<Requisition> requisitionsToApprove)
        {
            return depEmployees.Select(x => x.DepEmployeeID).Intersect(requisitionsToApprove.Select(x => x.EmployeeID)).ToList();
        }

        public List<RequisitionDetail> ListRequisitionDetailsByReqID(int reqID)
        {
            return db.RequisitionDetails.Where(x => x.RequisitionID == reqID).ToList();
        }

        public Requisition FindRequisitionByReqID(int reqID)
        {
            return db.Requisitions.Where(x => x.RequisitionID == reqID).First();
        }

        public string FindNameByID(int userID)
        {
            return db.DepEmployees.Where(x => x.DepEmployeeID == userID).Select(x => x.FirstName + " " + x.LastName).First().ToString();
        }
    }
}