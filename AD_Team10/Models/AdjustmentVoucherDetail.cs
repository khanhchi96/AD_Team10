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
<<<<<<< HEAD
        public int Quantity { get; set; }
=======
        public string Quantity { get; set; }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        public string Reason { get; set; }

        public virtual AdjustmentVoucher AdjustmentVoucher { get; set; }
        public virtual Item Item { get; set; }
    }
}