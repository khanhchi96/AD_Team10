using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
<<<<<<< HEAD
using AD_Team10.Models;
using AD_Team10.DAL;

namespace AD_Team10.Controllers.Department
{
    [Authorize(Roles = "ACTINGHEAD, EMPLOYEE")]
    public class EmployeeController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            DepUser depUser = db.DepUsers.Where(x => x.Username == username).First();
            DepartmentRole currentRole = db.DepUsers.Where(x => x.Username == username).Select(x => x.Role).First();

            DateTime? startDate = db.DepUsers.Where(x => x.Username == username).Select(x => x.StartDate).FirstOrDefault();

            if (startDate != null && currentRole == DepartmentRole.EMPLOYEE)
            {
                DateTime today = DateTime.Today;
                if (today == startDate)
                {
                    depUser.Role = DepartmentRole.ACTINGHEAD;
                    db.SaveChanges();
                    currentRole = db.DepUsers.Where(x => x.Username == username).Select(x => x.Role).First();
                    return RedirectToAction("Index", "ActingHead");
                }
            }

            ViewBag.role = currentRole;

=======

namespace AD_Team10.Controllers.Department
{
    [Authorize(Roles = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
            return View("~/Views/Department/Employee/Index.cshtml");
        }
    }
}