using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AD_Team10.Models;

namespace AD_Team10.Authentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties  

        public string Username { get; set; }
<<<<<<< HEAD
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public int UserID { get; set; }
=======
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b

        #endregion

        public CustomMembershipUser(StoreUser user) : base("CustomMembership", user.Username, user.StoreEmployeeID, user.StoreEmployee.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username;
            FirstName = user.StoreEmployee.FirstName;
            LastName = user.StoreEmployee.LastName;
            Role = user.Role.ToString();
<<<<<<< HEAD
            UserID = user.StoreUserID;
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        }

        public CustomMembershipUser(DepUser user) : base("CustomMembership", user.Username, user.DepEmployeeID, user.DepEmployee.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username;
            FirstName = user.DepEmployee.FirstName;
            LastName = user.DepEmployee.LastName;
            Role = user.Role.ToString();
<<<<<<< HEAD
            UserID = user.DepEmployeeID;
=======
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
        }
    }
}