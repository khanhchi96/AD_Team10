using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class RetrievalListDetail
    {
        [Key, Column(Order = 0), ForeignKey("RetrievalList")]
        public int RetrievalListID { get; set; }
        [Key, Column(Order = 1), ForeignKey("Department")]
        public int DepartmentID { get; set; }
        [Key, Column(Order = 2), ForeignKey("Item")]
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int QuantityReceived { get; set; }

        public virtual RetrievalList RetrievalList { get; set; }
        public virtual Department Department { get; set; }
        public virtual Item Item { get; set; }

    }
}