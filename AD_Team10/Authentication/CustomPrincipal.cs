using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace AD_Team10.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int UserID { get; set; }

        public IIdentity Identity
        {
            get; private set;
        }

        
        public bool IsInRole(string role)
        {
            if (Role.Equals(role))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}