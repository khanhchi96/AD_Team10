using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class RetrievalList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RetrievalListID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Requisition> Requisitions { get; set; }
        public virtual ICollection<RetrievalListDetail> RetrievalListDetails { get; set; }

    }
}