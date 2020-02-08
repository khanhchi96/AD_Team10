using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Purchase Order ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderID{ get; set; }
        [ForeignKey("Supplier")]
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }
        [Display(Name = "Status")]
        public OrderStatus OrderStatus { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}