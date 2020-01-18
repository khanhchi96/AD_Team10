using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public enum DepartmentRole
    {
        EMPLOYEE, REPRESENTATIVE, HEAD
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

        public virtual DepEmployee DepEmployee { get; set; }
    }
}