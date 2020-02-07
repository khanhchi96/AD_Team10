using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class StoreEmployee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreEmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }

        public override string ToString()
        {
            if (MiddleName != null) return FirstName + " " + MiddleName + " " + LastName;
            else return FirstName + " " + LastName;
        }
    }
}
