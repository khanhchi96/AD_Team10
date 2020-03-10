using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using AD_Team10.DAL;

//Author: Phung Khanh Chi
namespace AD_Team10.Authentication
{
    public class CustomRole : RoleProvider
    {
        public string UserType { get; set; }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

         
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (DBContext dbContext = new DBContext())
            {
                if (UserType == "Store")
                {
                    var selectedUser = (from us in dbContext.StoreUsers.Include("Role")
                                        where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                        select us).FirstOrDefault();
                    if (selectedUser != null)
                    {
                        userRoles = new[] { selectedUser.Role.ToString() };
                    }
                }
                else if (UserType == "Department")
                {
                    var selectedUser = (from us in dbContext.StoreUsers.Include("Role")
                                        where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                        select us).FirstOrDefault();
                    if (selectedUser != null)
                    {
                        userRoles = new[] { selectedUser.Role.ToString() };
                    }
                }
                return userRoles.ToArray();
            }


        }

        public override string Name => base.Name;

        public override string Description => base.Description;

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

       
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}