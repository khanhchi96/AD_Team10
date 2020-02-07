using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
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
<<<<<<< HEAD
        [Display(Name = "Purchase Order ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderID{ get; set; }
        [ForeignKey("Supplier")]
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Completed Date")]
        public DateTime CompletedDate { get; set; }
        [Display(Name = "Status")]
=======
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderID{ get; set; }
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public DateTime OrderDate { get; set; }
       
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        public OrderStatus? OrderStatus { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}