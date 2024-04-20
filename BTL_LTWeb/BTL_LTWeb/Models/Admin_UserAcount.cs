using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_LTWeb.Models
{
    public class Admin_UserAcount
    {
        public bool isDeleted { get; set; }
        public string account_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string adr { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}