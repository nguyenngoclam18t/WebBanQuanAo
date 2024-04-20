using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BTL_LTWeb.Models
{
    public class RegisterViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string adr { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}