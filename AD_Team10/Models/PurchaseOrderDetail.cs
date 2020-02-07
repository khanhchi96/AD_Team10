using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class PurchaseOrderDetail
    {
        [Key, Column(Order = 0), ForeignKey("PurchaseOrder")]
        public int OrderID { get; set; }
        [Key, Column(Order = 1), ForeignKey("Item")]
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int QuantityReceived { get; set; }
        public float Price { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Item Item { get; set; }
    }
}
