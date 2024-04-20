using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Services;
using BTL_LTWeb.Models;

namespace BTL_LTWeb.Controllers
{
    public class ProductController : Controller
    {
        private ProductService productService = new ProductService();
        // GET: Product
        public ActionResult ShowProducts(int? pageNum, int? min, int? max, string name)
        {
            List<Product> allProducts = productService.GetProductsByName(name);
            decimal count = allProducts.Count();
            decimal pageSize = Math.Ceiling(count / ProductService.PAGE_SIZE);

            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = pageNum;
            ViewBag.nextPage = pageNum + 1;
            ViewBag.prevPage = pageNum - 1;
            ViewBag.min = min;
            ViewBag.max = max;
            ViewBag.name = name;
            if (pageNum != null && min != null && max != null)
                return ShowProductsByPageAndPrice(pageNum, min, max, name);
            if (pageNum != null)
                return ShowProductsByPage(pageNum, name);

            return ShowProductsByPage(1, name);
        }

        public ActionResult ShowProductsByPage(int? pageNum, string name)
        {
            List<Product> allProducts = productService.GetProducts();
            int count = allProducts.Count();
            decimal page = count / ProductService.PAGE_SIZE;
            decimal pageSize = Math.Ceiling(page);

            List<Product> products = productService.GetProductsByPage(pageNum, name);
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = pageNum;
            ViewBag.nextPage = pageNum + 1;
            ViewBag.prevPage = pageNum - 1;
            ViewBag.name = name;
            return View("ShowProducts", products);
        }

        public ActionResult ShowProductsByPageAndPrice(int? pageNum, int? min, int? max, string name)
        {
            List<Product> allProducts = productService.GetProductsByPrice(min, max, name);
            decimal count = allProducts.Count();
            decimal pageSize = Math.Ceiling(count / ProductService.PAGE_SIZE);

            List<Product> products = productService.GetProductsByPageAndPrice(pageNum, min, max, name);
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = pageNum;
            ViewBag.nextPage = pageNum + 1;
            ViewBag.prevPage = pageNum - 1;
            ViewBag.min = min;
            ViewBag.max = max;
            return View("ShowProducts", products);
        }

        [HttpPost]
        public ActionResult ShowProductsByName(int? pageNum, string name)
        {
            List<Product> allProducts = productService.GetProductsByName(name);
            decimal count = allProducts.Count();
            decimal pageSize = Math.Ceiling(count / ProductService.PAGE_SIZE);

            List<Product> products = productService.GetProductsByPageAndName(pageNum, name);
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = pageNum;
            ViewBag.nextPage = pageNum + 1;
            ViewBag.prevPage = pageNum - 1;
            ViewBag.name = name;
            return View("ShowProducts", products);
        }

        public ActionResult ProductDetail(int id)
        {
            var product = productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View("ProductDetail", product);
        }
        public ActionResult GetProduct()
        {
            List<Product> product = productService.GetProducts();
            return View(product);
        }
    }
}