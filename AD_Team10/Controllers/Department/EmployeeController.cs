using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
