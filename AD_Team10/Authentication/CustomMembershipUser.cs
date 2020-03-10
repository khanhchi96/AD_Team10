using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AD_Team10.Models;

//Author: Phung Khanh Chi
namespace AD_Team10.Authentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties  

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public int UserID { get; set; }

        #endregion

        public CustomMembershipUser(StoreUser user) : base("CustomMembership", user.Username, user.StoreEmployeeID, user.StoreEmployee.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username;
            Password = user.Password;
            FirstName = user.StoreEmployee.FirstName;
            LastName = user.StoreEmployee.LastName;
            Role = user.Role.ToString();
            UserID = user.StoreUserID;
        }

        public CustomMembershipUser(DeptUser user) : base("CustomMembership", user.Username, user.DeptEmployeeID, user.DeptEmployee.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username;
            Password = user.Password;
            FirstName = user.DeptEmployee.FirstName;
            LastName = user.DeptEmployee.LastName;
            Role = user.Role.ToString();
            UserID = user.DeptEmployeeID;
        }
    }
}