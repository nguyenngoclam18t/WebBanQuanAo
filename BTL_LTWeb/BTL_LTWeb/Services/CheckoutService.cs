using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Services
{
    public class CheckoutService : BaseService
    {
        public List<Cart> getCartByAccountName(string accountName)
        {
            return db.Carts.ToList();
        }

        public List<CartDetail> getCartDetailsBySession()
        {
            return HttpContext.Current.Session["GioHang"] as List<CartDetail>;
        }

        public void changeCartToInvoiceDetails(string accountName)
        {
            string invoiceId = this.getInvoiceId();
            //List<Cart> carts = this.getCartByAccountName(accountName);
            List<CartDetail> carts = this.getCartDetailsBySession();

            Invoice newInvoice = new Invoice();

            newInvoice.invoice_id = invoiceId;
            newInvoice.account_name = accountName;
            newInvoice.invoice_date = DateTime.Now;
            newInvoice.total_amount = 0;
            newInvoice.status = "Pending";

            db.Invoices.Add(newInvoice);
            db.SaveChanges();

            foreach (CartDetail cart in carts)
            {
                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.invoice_id = newInvoice.invoice_id;
                invoiceDetail.product_id = cart.ID;
                invoiceDetail.quantity = cart.Quantity;
                invoiceDetail.color = cart.Color;
                invoiceDetail.size = cart.Size;
                invoiceDetail.total_price = (decimal) cart.Total;

                db.InvoiceDetails.Add(invoiceDetail);
                db.SaveChanges();

                //db.Carts.Remove(cart);
                //db.SaveChanges();
            }

            newInvoice.total_amount = this.getTotalPrice(invoiceId);
            db.SaveChanges();
        }

        private decimal getTotalPrice(string invoiceId)
        {
            List<InvoiceDetail> invoiceDetails = db.InvoiceDetails.Where(i => i.invoice_id == invoiceId).ToList();
            decimal sum = 0;
            foreach (InvoiceDetail invoiceDetail in invoiceDetails)
            {
                sum += (decimal) invoiceDetail.total_price;
            }

            return sum;
        }

        private string getInvoiceId()
        {
            List<Invoice> invoices = db.Invoices.ToList();
            Invoice lastestInvoice = invoices[invoices.Count - 1];

            string lastestId = lastestInvoice.invoice_id;

            string numId = lastestId.Substring(2);
            int numIdInt = int.Parse(numId);

            numIdInt++;

            string newInvoiceId = "HD" + numIdInt.ToString("D3");

            return newInvoiceId;
        }
    }
}