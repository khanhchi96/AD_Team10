﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AD_Team10.Models;

namespace AD_Team10.ViewModels
{
    public class RequisitionReport
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public virtual List<int> CategoryList { get; set; }
        public virtual List<int> DepartmentList { get; set; }
    }
}