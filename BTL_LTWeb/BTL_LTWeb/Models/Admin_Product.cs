using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_LTWeb.Models
{
    public class Admin_Product
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Descript { get; set; }
        public decimal Price { get; set; }
        public string Img { get; set; }
    }
}