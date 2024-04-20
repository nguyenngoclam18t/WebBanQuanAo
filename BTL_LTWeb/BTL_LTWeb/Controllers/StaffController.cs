using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using System.Data.Entity;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class StaffController : Controller
    {
        BaseService data = new BaseService();
        // GET: Staff
        public ActionResult Index()
        {
            List<Account> account = data.db.Accounts
                .Where(acc => acc.role == "User")
                .Include(d => d.User).ToList();
            return View(account);
        }
        public ActionResult Product()
        {

            List<Product> product = data.db.Products.ToList();
            return View(product);
        }

        public ActionResult Invoice()
        {

            List<Invoice> invoice = data.db.Invoices.Where(i => i.status == "Pending").ToList();
            return View(invoice);
        }
        public ActionResult InvoiceByAccount(string accountname)
        {
            List<Invoice> invoice = data.db.Invoices.Where(i => i.account_name == accountname).ToList();
            return View(invoice);
        }


        public ActionResult Invoice_Details(string invoice_id)
        {
            var invoiceDetail = data.db.InvoiceDetails
                                .Where(d => d.invoice_id == invoice_id)
                                .Include(d => d.Product)
                                .ToList();

            return View(invoiceDetail);
        }

        /* [HttpGet]
         public ActionResult Update(int masp, string name, string description)
         {
             // Update product information in the database
             var productToUpdate = BaseService.db.Products.Find(masp);

             if (productToUpdate != null)
             {
                 productToUpdate.product_name = name;
                 productToUpdate.description = description;

                 BaseService.db.SaveChanges();

                 // Redirect to the product list page
                 return RedirectToAction("Product");
             }
             else
             {
                 // Display an error message if the product is not found
                 return View("Error", new { message = "Product not found" });
             }
         }*/
        [HttpPost]
        public ActionResult Update(int masp, string name, string description)
        {
            try
            {
                // Update product information in the database
                var productToUpdate = data.db.Products.Find(masp);

                if (productToUpdate != null)
                {
                    productToUpdate.product_name = name;
                    productToUpdate.description = description;

                    data.db.SaveChanges();

                    // Return a success status
                    return Json(new { success = true });
                }
                else
                {
                    // Return an error status if the product is not found
                    return Json(new { success = false, message = "Product not found" });
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}