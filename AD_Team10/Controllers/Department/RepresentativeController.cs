using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
<<<<<<< HEAD
using AD_Team10.Authentication;
namespace AD_Team10.Controllers.Department
{
    [CustomAuthorize(Roles = "REPRESENTATIVE")]
=======

namespace AD_Team10.Controllers.Department
{
    [Authorize(Roles = "REPRESENTATIVE")]
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
    public class RepresentativeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Department/Representative/Index.cshtml");
        }
    }
}