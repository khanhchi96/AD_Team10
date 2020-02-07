using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}
