using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTL_LTWeb.Models;
using System.Data.Entity;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class Staff_APIController : ApiController
    {
        BaseService data = new BaseService();

        [HttpOptions]
        public int SubmitInvoice(string invoice_id)
        {

            var invoiceToUpdate = data.db.Invoices.Find(invoice_id);

            if (invoiceToUpdate != null)
            {
                invoiceToUpdate.status = "Success";

                data.db.SaveChanges();
                return 1;
            }
            else
            {
                return 2;
            }
        }

        [HttpGet]
        public IHttpActionResult UpdateProduct(int product_id)
        {
            var product = data.db.Products.Find(product_id);

            return Json(new
            {
                success = true,
                product_id = product.product_id,
                product_name = product.product_name,
                description = product.description,
                price = product.price,
                image_url = product.image_url
            });
        }

        [HttpPost]
        public int CancelInvoice(string invoice_id)
        {

            var invoiceToUpdate = data.db.Invoices.Find(invoice_id);

            if (invoiceToUpdate != null)
            {
                invoiceToUpdate.status = "Cancel";

                data.db.SaveChanges();
                return 1;
            }
            else
            {
                return 2;
            }
        }

    }
}

