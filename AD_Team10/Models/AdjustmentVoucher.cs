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
    public enum VoucherStatus
    {
        Pending, Supervisor_Approved, Manager_Approved, Rejected
    }
    public class AdjustmentVoucher
    {
<<<<<<< HEAD
        [Display(Name = "Voucher ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdjustmentVoucherID { get; set; }
        [Display(Name = "Adjustment Date")]
        public DateTime AdjustmentDate { get; set; }
        [Display(Name = "Status")]
        public VoucherStatus Status { get; set; }
        [ForeignKey("Clerk")]
        public int StoreEmployeeID { get; set; }
        public string Remark { get; set; }

        [Display(Name = "Clerk")]
        public virtual StoreEmployee Clerk { get; set; }
        public virtual ICollection<AdjustmentVoucherDetail> AdjustmentVoucherDetails { get; set; }
=======
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdjustmentVoucherID { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public VoucherStatus Status { get; set; }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
    }
}