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
<<<<<<< HEAD
        public int QuantityReceived { get; set; }
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        public float Price { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Item Item { get; set; }
    }
}