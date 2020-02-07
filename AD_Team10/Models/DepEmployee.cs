using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class DepEmployee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepEmployeeID { get; set; }
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }

        public virtual Department Department { get; set; }

<<<<<<< HEAD
        public override string ToString()
=======
        public override String ToString()
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        {
            if (MiddleName != null) return FirstName + MiddleName + LastName;
            else return FirstName + LastName;
        }
    }
}