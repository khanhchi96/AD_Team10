using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public enum Status
    {
        Pending, Approved, Rejected, Incompleted, Completed
    }
    public class Requisition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionID { get; set; }
        public DateTime? RequisitionDate { get; set; }
        public Status? Status { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        public virtual DepEmployee Employee { get; set; }
    
        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }
    }
}