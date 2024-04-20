using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BTL_LTWeb.Models
{
    public class LoginViewModel
    {
        public string accountname { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string isdeleted { get; set; }
    }
}