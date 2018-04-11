using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class UsersRole
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}