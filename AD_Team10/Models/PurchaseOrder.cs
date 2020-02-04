using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public enum OrderStatus
    {
        Pending, Delivering, Incompleted, Completed 
    }
    public class PurchaseOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderID{ get; set; }
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CompletedDate { get; set; }
       
        public OrderStatus? OrderStatus { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}