using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomDisbursementList
    {
        public int RetrievalListID { get; set; }
        public int DepartmentID { get; set; }
        public CustomItem[] CustomItems { get; set; }     
    }
}