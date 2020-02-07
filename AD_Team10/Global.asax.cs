using AD_Team10.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
<<<<<<< HEAD
using AD_Team10.ViewModels;
using System.Web.Http;
using AD_Team10.App_Start;
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b

namespace AD_Team10
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
<<<<<<< HEAD
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
=======
            AreaRegistration.RegisterAllAreas();
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomPrincipal>(authTicket.UserData);

                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);

                principal.Username = serializeModel.Username;
<<<<<<< HEAD
                principal.Password = serializeModel.Password;
                principal.FirstName = serializeModel.FirstName;
                principal.LastName = serializeModel.LastName;
                principal.MiddleName = serializeModel.MiddleName;
                principal.Role = serializeModel.Role;
                principal.UserID = serializeModel.UserID;
=======
                principal.FirstName = serializeModel.FirstName;
                principal.LastName = serializeModel.LastName;
                principal.Role = serializeModel.Role;
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b

                HttpContext.Current.User = principal;
            }

        }
    }
}
