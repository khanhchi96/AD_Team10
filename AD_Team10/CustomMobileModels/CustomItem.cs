using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomItem
    {
        public int ItemID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int QuantityReceived { get; set; }
    }
}