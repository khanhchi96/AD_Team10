using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "REPRESENTATIVE")]
    public class RepresentativeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Department/Representative/Index.cshtml");
        }
    }
}