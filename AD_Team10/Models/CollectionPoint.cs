using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Models
{
    public class CollectionPoint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollectionPointID { get; set; }
        public string CollectionPointName { get; set; }
        public virtual ICollection<Department> Departments { get; set; }

        public List<CollectionPoint> listCP { get; set; }
        public IEnumerable<SelectListItem> CPs
        {
            get
            {
                return new SelectList(listCP, "CollectionPointID", "CollectionPointName");
            }
        }
    }
}