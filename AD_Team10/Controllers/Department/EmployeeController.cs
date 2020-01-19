﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers.Department
{
    [Authorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Department/Employee/Index.cshtml");
        }
    }
}