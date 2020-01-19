using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers.Store
{
    [Authorize(Roles = "SUPERVISOR")]
    public class SupervisorController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Store/Supervisor/Index.cshtml");
        }
    }
}