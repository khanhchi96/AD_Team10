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
    public class Item
    {
<<<<<<< HEAD
        [Display(Name = "Item ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }
        public string Description { get; set; }
        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }
        [Display(Name = "Reorder Quantity")]
        public int ReorderQuantity { get; set; }
        [Display(Name = "Units in Stock")]
=======
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        public int UnitsInStock { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<SupplierItem> SupplierItems { get; set; }
        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }
    
    }
}