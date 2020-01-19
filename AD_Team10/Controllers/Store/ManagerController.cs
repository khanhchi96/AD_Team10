using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers.Store
{
    [Authorize(Roles = "MANAGER")]
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Store/Manager/Index.cshtml");
        }
    }
}