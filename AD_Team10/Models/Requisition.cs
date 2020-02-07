using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Requisition ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionID { get; set; }
        [Display(Name = "Approval Date")]
        public DateTime ApprovalDate { get; set; }
        [Display(Name = "Requisition Date")]
        public DateTime RequisitionDate { get; set; }
        [Display(Name = "Completed Date")]
        public DateTime CompletedDate { get; set; }
        public Status? Status { get; set; }
        public string Remark { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        [ForeignKey("RetrievalList")]
        public int RetrievalListID { get; set; }

        public virtual DepEmployee Employee { get; set; }
        public virtual RetrievalList RetrievalList { get; set; }

        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }
    }
}