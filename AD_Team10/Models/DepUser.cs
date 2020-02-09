using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    // add ActingHead role here
    public enum DepartmentRole
    {
        EMPLOYEE, REPRESENTATIVE, HEAD, ACTINGHEAD
    }
    public class DepUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepUserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DepartmentRole Role { get; set; }
        [ForeignKey("DepEmployee")]
        public int DepEmployeeID { get; set; }
        //add two more attributes startdate and enddate here based Steph's head codes
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual DepEmployee DepEmployee { get; set; }
    }
}