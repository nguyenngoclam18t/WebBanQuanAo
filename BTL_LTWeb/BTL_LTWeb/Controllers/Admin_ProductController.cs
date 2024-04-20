using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class Admin_ProductController : Controller
    {
        // GET: Admin_Product
        string constring = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
    
        SqlConnection con;
        int pageSize = 5;
        public ActionResult ViewProduct(int?page, string nameProduct = "")
        {

            int pageNumber = (page ?? 0);
            BaseService QL = new BaseService();
            List<Product> list = QL.db.Products.Where(p => p.product_name.Contains(nameProduct)).OrderBy(p => p.product_id).ToList();
            int totalCount = list.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));
            list = list.Skip((pageNumber-1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.NameProduct = nameProduct;
            return View(list);
        }

        [HttpPost]
        public ActionResult ViewProduct(string nameProduct = "",int? page =1)
        {
            return RedirectToAction("ViewProduct",new {page=page,nameProduct=nameProduct });
        }
        public ActionResult Delete(int id)
        {
            BaseService QL = new BaseService();
            var product = QL.db.Products.Find(id);
            if (product == null)
            {
                TempData["SuccessMessage"] = "Product deleted failed.";
                return RedirectToAction("Product");
            }
            try
            {
                con = new SqlConnection(constring);
                con.Open();
                string query = " delete Categories_Products where product_id= @id";
                SqlCommand sqlcmd = new SqlCommand(query, con);
                sqlcmd.Parameters.AddWithValue("@id", id);
                sqlcmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            try
            {
                var warehousesToDelete = QL.db.Warehouses.Where(w => w.product_id == id).ToList();
                foreach (var warehouse in warehousesToDelete)
                {
                    QL.db.Warehouses.Remove(warehouse);
                }
                QL.db.Products.Remove(product);
                QL.db.SaveChanges();
                TempData["SuccessMessage"] = "Product deleted successfully.";
            }
            catch (Exception)
            {
                TempData["SuccessMessage"] = "Product deleted failed.";
              
            }
            
            return RedirectToAction("ViewProduct");
        }

       
    }
}