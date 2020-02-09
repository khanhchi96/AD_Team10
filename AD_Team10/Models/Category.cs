using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Item> Items { get; set; }

        public List<Category> listCat { get; set; }
        public IEnumerable<SelectListItem> Cats
        {
            get
            {
                return new SelectList(listCat, "CategoryId", "CategoryName");
            }
        }
    }
}