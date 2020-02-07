using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace AD_Team10.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
<<<<<<< HEAD
        public string Username { get; set; }
        public string Password { get; set; }
=======
        public int Username { get; set; }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
<<<<<<< HEAD
        public int UserID { get; set; }
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b

        public IIdentity Identity
        {
            get; private set;
        }

<<<<<<< HEAD
        
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
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