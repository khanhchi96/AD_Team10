using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AD_Team10.Models
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        [ForeignKey("CollectionPoint")]
        public int CollectionPointID { get; set; }

        public virtual CollectionPoint CollectionPoint { get; set; }
        public virtual ICollection<DeptEmployee> Employees { get; set; }

    }
}