using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.ViewModels;
using System.Web.Security;
using AD_Team10.Models;
using Newtonsoft.Json;
using AD_Team10.Authentication;

namespace AD_Team10.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
<<<<<<< HEAD
            if (System.Web.HttpContext.Current.User.GetType() == typeof(RolePrincipal))
                return View();
            else
            {
                CustomPrincipal user = (CustomPrincipal) System.Web.HttpContext.Current.User;
                string controller = user.Role.ToString()[0] + user.Role.ToString().Substring(1).ToLower();
                return RedirectToAction("Index", controller);
            }
=======
            return View();
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
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
<<<<<<< HEAD
        public ActionResult Login(LoginView loginView)
=======
        public ActionResult Login(LoginView loginView, string userType)
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        {
            if (ModelState.IsValid)
            {
                CustomMembership customMembership = new CustomMembership
                {
<<<<<<< HEAD
                    UserType = loginView.UserType
                };
                CustomRole customRole = new CustomRole
                {
                    UserType = loginView.UserType
=======
                    UserType = userType
                };
                CustomRole customRole = new CustomRole
                {
                    UserType = userType
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
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
<<<<<<< HEAD
            return RedirectToAction("Index", "Home", null);
=======
            return RedirectToAction("Login", "Account", null);
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}