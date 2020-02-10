using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class DataTableList
    {
        public List<DataRow> List { get; set; }//Regular list to hold data from Datatable
        public PagedList<DataRow> Plist { get; set; }
    }
}