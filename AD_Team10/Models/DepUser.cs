using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public enum DepartmentRole
    {
<<<<<<< HEAD
        EMPLOYEE, REPRESENTATIVE, HEAD, ACTINGHEAD
=======
        EMPLOYEE, REPRESENTATIVE, HEAD
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
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
<<<<<<< HEAD
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b

        public virtual DepEmployee DepEmployee { get; set; }
    }
}