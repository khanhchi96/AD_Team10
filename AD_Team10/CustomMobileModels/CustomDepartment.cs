using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AD_Team10.Models;

namespace AD_Team10.CustomMobileModels
{
    public class CustomDepartment
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
        public CustomDeptEmployee Representative { get; set; }
    }
}