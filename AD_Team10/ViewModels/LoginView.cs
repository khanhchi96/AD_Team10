using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class LoginView
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    //public class CustomSerializeModel
    //{
    //    public string Username { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string RoleName { get; set; }

    //}
}