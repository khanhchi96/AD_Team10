using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class RequisitionDetail
    {
        [Key, Column(Order = 0), ForeignKey("Requisition")]
        public int RequisitionID { get; set; }
        [Key, Column(Order = 1), ForeignKey("Item")]
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int QuantityDelivered { get; set; }

        public virtual Requisition Requisition { get; set; }
        public virtual Item Item { get; set; }
    }
}