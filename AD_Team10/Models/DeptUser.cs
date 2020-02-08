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
    public class DeptUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeptUserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DepartmentRole Role { get; set; }
        [ForeignKey("DeptEmployee")]
        public int DeptEmployeeID { get; set; }

        public virtual DeptEmployee DeptEmployee { get; set; }
    }
}