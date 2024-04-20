using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Services;
using BTL_LTWeb.Models;

namespace BTL_LTWeb.Controllers
{
    public class HomeController : Controller
    {
        
        private ProductService productService = new ProductService();
        public ActionResult Index(int? categoryId)
        {
            
            List<Product> products = null;
            if (categoryId != null)
            {
                products = productService.GetProductByCategoryId(categoryId);
                ViewBag.categoryId = categoryId;
                return View(products);
            }

            products = productService.GetProducts();
            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}