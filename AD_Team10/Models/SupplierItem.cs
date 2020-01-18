using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class SupplierItem
    {
        [Key, Column(Order = 0), ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        [Key, Column(Order = 1), ForeignKey("Item")]
        public int ItemID { get; set; }
        public float Price { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual Item Item { get; set; }
    }
}