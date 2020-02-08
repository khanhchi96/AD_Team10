using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomRequisition
    {
        public int RequisitionID { get; set; }
        public string RequisitionDate { get; set; }
        public string EmployeeName { get; set; }
        public CustomItem[] CustomItems { get; set; }
    }
}