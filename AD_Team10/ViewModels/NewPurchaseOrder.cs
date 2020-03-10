using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class NewPurchaseOrder
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int SupplierID { get; set; }
        public float Price { get; set; }
    }
}