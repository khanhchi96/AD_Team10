using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomPurchaseOrder
    {
        public int OrderID { get; set; }
        public string SupplierName { get; set; }
        public string Status { get; set; }
        public string OrderDate { get; set; }
        public CustomItem[] CustomItems { get; set; }
    }
}