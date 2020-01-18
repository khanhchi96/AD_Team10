using System;
using System.Collections.Generic;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdjustmentVoucherID { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public VoucherStatus Status { get; set; }
    }
}