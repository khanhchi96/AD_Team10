using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomRetrievalListDetail
    {
        //public CustomDepartment Department { get; set; }
        //public CustomItem Item { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int Quantity { get; set; }
        public int QuantityOffered { get; set; }
    }
}