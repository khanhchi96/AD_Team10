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


//Author: Phung Khanh Chi
namespace AD_Team10.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.GetType() == typeof(RolePrincipal))
                return View();
            else
            {
                CustomPrincipal user = (CustomPrincipal) System.Web.HttpContext.Current.User;
                string role = user.Role.ToString().Replace("ACTING", "");
                string controller = role[0] + role.Substring(1).ToLower();
                return RedirectToAction("Index", controller);
            }
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
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                CustomMembership customMembership = new CustomMembership
                {
                    UserType = loginView.UserType
                };
                CustomRole customRole = new CustomRole
                {
                    UserType = loginView.UserType
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
                        string role = user.Role.ToString().Replace("ACTING", "");
                        controller = role[0] + role.Substring(1).ToLower();
                    }
                    return RedirectToAction("Index", controller);
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid");
            ViewBag.UserType = loginView.UserType;
            return View(loginView);
        }


        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "")
            {
                Expires = DateTime.Now.AddYears(-1)
            };
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}