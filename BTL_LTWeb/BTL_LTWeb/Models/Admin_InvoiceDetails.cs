using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_LTWeb.Models
{
    public class Admin_InvoiceDetails
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Img { get; set; }
        public int Quantity { get; set; }
        public decimal total { get; set; }

    }
}