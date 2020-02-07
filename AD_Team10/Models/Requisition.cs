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
    public enum Status
    {
        Pending, Approved, Rejected, Incompleted, Completed
    }
    public class Requisition
    {
<<<<<<< HEAD
        [Display(Name = "Requisition ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionID { get; set; }
        [Display(Name = "Approval Date")]
        public DateTime? ApprovalDate { get; set; }
        [Display(Name = "Requisition Date")]
        public DateTime RequisitionDate { get; set; }
        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }
        public Status? Status { get; set; }
        public string Remark { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        //[ForeignKey("RetrievalList")]
        public int RetrievalListID { get; set; }

        public virtual DepEmployee Employee { get; set; }
        public virtual RetrievalList RetrievalList { get; set; }

=======
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionID { get; set; }
        public DateTime RequisitionDate { get; set; }
        public Status? Status { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        public virtual DepEmployee Employee { get; set; }
    
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }
    }
}