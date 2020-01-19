using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AD_Team10.Models
{
    public enum StoreRole
    {
        CLERK, MANAGER, SUPERVISOR
    }
    public class StoreUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreUserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public StoreRole Role { get; set; }
        [ForeignKey("StoreEmployee")]
        public int StoreEmployeeID { get; set; }
        public virtual StoreEmployee StoreEmployee { get; set; }
    }
}