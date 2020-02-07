using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class ReportByTimeSeries
    {
        public ReportByTimeSeriesKey Key { get; set; }
        public double Quantity { get; set; }
    }

    public class ReportByTimeSeriesKey
    {
        public int? Month { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
    }
}