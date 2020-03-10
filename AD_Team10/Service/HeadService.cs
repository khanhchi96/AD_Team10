using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AD_Team10.DAL;
using AD_Team10.Models;

//Author: Stephanie Cheng Sin Yin
namespace AD_Team10.Service
{
    public class HeadService
    {
        private DBContext db = new DBContext();
        public string FindUserByUsername()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public List<DeptEmployee> ListDepartmentEmployees(int depID)
        {
            return db.DeptEmployees.Where(x => x.DepartmentID == depID).ToList();
        }

        public int FindUserID()
        {
            string username = HttpContext.Current.User.Identity.Name;
            return db.DeptUsers.Where(x => x.Username == username).Select(x => x.DeptEmployeeID).First();
        }

        public int FindDepID()
        {
            int userID = FindUserID();
            return db.DeptEmployees.SingleOrDefault(x => x.DeptEmployeeID == userID).DepartmentID;
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
            return db.DeptUsers.SingleOrDefault(x => x.DeptUserID == userID);

        }

        public List<Requisition> ListRequisitionsByStatus(string status)
        {
            return db.Requisitions.Where(x => x.Status.ToString() == status).OrderBy(r => r.RequisitionDate).ToList();
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

        public void AssignRep(int DeptEmployeeID)
        {
            int depID = FindDepID();

            //change current rep to emp
            int repID = FindRepID(depID);
            DeptUser currentRep = FindDepUser(repID);
            currentRep.Role = DepartmentRole.EMPLOYEE;

            //assign selected emp as rep
            DeptUser newRep = FindDepUser(DeptEmployeeID);
            newRep.Role = DepartmentRole.REPRESENTATIVE;

            db.SaveChanges();
        }

        public void AssignActingHead(int DeptEmployeeID, DateTime start, DateTime end)
        {
            DeptUser depUser = db.DeptUsers.SingleOrDefault(d => d.DeptEmployeeID == DeptEmployeeID);
            depUser.StartDate = start;
            depUser.EndDate = end;
            DateTime today = DateTime.Today;

            if (start == today)
            {
                depUser.Role = DepartmentRole.ACTINGHEAD;
            }

            db.SaveChanges();

        }

        public void CancelActingHead(int actingHeadID)
        {
            DeptUser depUser = FindDepUser(actingHeadID);
            depUser.Role = DepartmentRole.EMPLOYEE;
            depUser.StartDate = null;
            depUser.EndDate = null;
            db.SaveChanges();
        }

        public string ApproveRejectRequisition(int reqID, string remarks, string button)
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
            return message;
        }

    }
}