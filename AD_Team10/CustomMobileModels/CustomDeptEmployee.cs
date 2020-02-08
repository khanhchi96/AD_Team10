using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomDeptEmployee
    {
        public int DepartmentID { get; set; }
        public int DeptEmployeeID { get; set; }
        public string DeptEmployeeName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}