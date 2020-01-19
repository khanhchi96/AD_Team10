using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers.Store
{
    [Authorize(Roles = "CLERK")]
    public class ClerkController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Store/Clerk/Index.cshtml");
        }
    }
}