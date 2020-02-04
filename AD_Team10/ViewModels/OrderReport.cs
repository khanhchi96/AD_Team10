using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class OrderReport
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<int> CategoryList { get; set; }
    }
}