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
<<<<<<< HEAD
        public string UserType { get; set; }
    }

    //public class CustomSerializeModel
    //{
    //    public string Username { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string RoleName { get; set; }

    //}
=======
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
}