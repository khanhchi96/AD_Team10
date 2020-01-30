using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;

namespace AD_Team10.Controllers.Store
{
    [CustomAuthorize(Roles = "SUPERVISOR")]
    public class SupervisorController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Store/Supervisor/Index.cshtml");
        }
    }
}