using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Authentication
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return ((CurrentUser != null && !CurrentUser.IsInRole(Roles)) || CurrentUser == null) ? false : true;
        }

<<<<<<< HEAD

=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
<<<<<<< HEAD
                        controller = "Home",
                        action = "Index",
=======
                        controller = "Account",
                        action = "Login",
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
<<<<<<< HEAD
                     controller = "Home",
=======
                     controller = "Account",
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }

    }
}