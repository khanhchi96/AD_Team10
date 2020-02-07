using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class AdjustmentVoucherDetail
    {
        [Key, Column(Order = 0), ForeignKey("AdjustmentVoucher")]
        public int VoucherID { get; set; }
        [Key, Column(Order = 1), ForeignKey("Item")]
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }

        public virtual AdjustmentVoucher AdjustmentVoucher { get; set; }
        public virtual Item Item { get; set; }
    }
}
