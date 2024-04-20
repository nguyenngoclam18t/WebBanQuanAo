using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_LTWeb.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AccountName { get; set; }
        public string Role { get; set; }
    }
}