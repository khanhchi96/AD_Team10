using AD_Team10.DAL;
using AD_Team10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AD_Team10.Authentication;

//Author: Phung Khanh Chi
namespace AD_Team10.Service
{
    public class RepresentativeService
    {

        private DBContext db = new DBContext();

        public List<Requisition> GetEmployeeRequisition()
        {
            int deptId = FindDepartmentId();
            List<Requisition> requisitions = db.Requisitions.Where(r => r.Employee.DepartmentID == deptId &&
                            (r.Status == Status.Approved || r.Status == Status.Incomplete || r.Status == Status.Completed))
                            .OrderByDescending(r => r.RequisitionID).ToList();
            return requisitions;
        }

        public int FindDepartmentId()
        {
            CustomPrincipal user = (CustomPrincipal)HttpContext.Current.User;
            int employeeId = user.UserID;
            int deptID = (db.DeptEmployees.Where(dE => dE.DeptEmployeeID == employeeId).SingleOrDefault()).DepartmentID;
            return deptID;
        }

        public List<RetrievalListDetail> GetDisbursementList(int retrievalListId)
        {
            int deptId = FindDepartmentId();
            List<RetrievalListDetail> retrievalListDetails = db.RetrievalListDetails
                                                            .Where(r => r.RetrievalListID == retrievalListId &&
                                                             r.DepartmentID == deptId).ToList();
            return retrievalListDetails;
        }

        

    }
}