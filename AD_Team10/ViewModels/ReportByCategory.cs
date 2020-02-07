using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class ReportByCategory
    {
        public ReportByCategoryKey Key { get; set; }
        public double Quantity { get; set; }
    }

    public class ReportByCategoryKey
    {
        public string Department { get; set; }
        public string Category { get; set; }
    }
}