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
        [Display(Name = "Adjustment Voucher ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdjustmentVoucherID { get; set; }
        [Display(Name = "Adjustment Date")]
        public DateTime AdjustmentDate { get; set; }
        [Display(Name = "Status")]
        public VoucherStatus Status { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<AdjustmentVoucherDetail> AdjustmentVoucherDetails { get; set; }
    }
}