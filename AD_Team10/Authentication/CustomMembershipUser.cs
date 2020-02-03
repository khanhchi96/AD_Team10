﻿using System;
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        #endregion

        public CustomMembershipUser(StoreUser user) : base("CustomMembership", user.Username, user.StoreEmployeeID, user.StoreEmployee.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username;
            FirstName = user.StoreEmployee.FirstName;
            LastName = user.StoreEmployee.LastName;
            Role = user.Role.ToString();
        }

        public CustomMembershipUser(DepUser user) : base("CustomMembership", user.Username, user.DepEmployeeID, user.DepEmployee.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username;
            FirstName = user.DepEmployee.FirstName;
            LastName = user.DepEmployee.LastName;
            Role = user.Role.ToString();
        }
    }
}