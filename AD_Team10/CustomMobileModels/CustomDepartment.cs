using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomDepartment
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string CollectionPointName { get; set; }
        public string RepName { get; set; }
        public string RepEmail { get; set; }
        public string RepPhone { get; set; }
    }
}