using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryService categoryService = new CategoryService();
        // GET: Category
        public ActionResult GetCategories()
        {
            List<Category> categories = categoryService.GetCategories();
            return View(categories);
        }

        
    }
}