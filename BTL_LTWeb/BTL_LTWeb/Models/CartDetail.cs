using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Models
{
    public class CartDetail
    {
        private BaseService baseService = new BaseService();   
        public int ID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Picture { get; set; }
        public int Quantity { get; set; }
        public double Total
        {
            get { return Quantity * Price; }
        }
        public CartDetail(int Ma, string selectedColor, string selectedSize)
        {
            ID = Ma;
            Product pro = baseService.db.Products.Single(s => s.product_id == ID);
            ProductName = pro.product_name;
            Picture = pro.image_url;
            Price = double.Parse(pro.price.ToString());
            Quantity = 1;
            Color = selectedColor;
            Size = selectedSize;
        }
        public CartDetail()
        {
        }
    }
}