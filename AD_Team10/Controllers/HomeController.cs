using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.ViewModels;
using System.Web.Security;
using AD_Team10.Models;
using AD_Team10.DAL;
using Newtonsoft.Json;
using AD_Team10.Authentication;

namespace AD_Team10.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public ActionResult Login(string userType)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.UserType = userType;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string userType)
        {
            if (ModelState.IsValid)
            {
                CustomMembership customMembership = new CustomMembership
                {
                    UserType = userType
                };
                CustomRole customRole = new CustomRole
                {
                    UserType = userType
                };

                if (customMembership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    string controller = "";
                    var user = (CustomMembershipUser)customMembership.GetUser(loginView.UserName, false);
                    
                    if (user != null)
                    {

                        string userData = JsonConvert.SerializeObject(user);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);
                        controller = user.Role.ToString()[0] + user.Role.ToString().Substring(1).ToLower();

                        DBContext db = new DBContext();
                        int employeeID=0;
                        switch (controller)
                        {
                            case "Employee":
                            case "Representative":
                            case "Head":
                                
                                var DepUser = (from depUser in db.DepUsers
                                               where depUser.Username == user.Username
                                               select depUser).FirstOrDefault();
                                employeeID = DepUser.DepEmployeeID;
                                break;

                            case "Clerk":
                            case "Manager":
                            case "Supervisor":
                               
                                var StoreUser = (from storeUser in db.StoreUsers
                                                 where storeUser.Username == user.Username
                                                 select storeUser).FirstOrDefault();
                                employeeID = StoreUser.StoreEmployeeID;
                                break;
                        }
                        return RedirectToAction("Index", controller, new { id = employeeID });
                    }
                    return RedirectToAction("Index", controller);
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid");
            return View(loginView);
        }


        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}