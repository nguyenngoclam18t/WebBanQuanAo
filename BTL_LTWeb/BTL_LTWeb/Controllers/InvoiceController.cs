using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class InvoiceController : Controller
    {
        InvoiceService invoiceService = new InvoiceService();
        // GET: Invoice
        public ActionResult GetInvoices()
        {
            Account currentAccount = Session["account"] as Account;
            List<Invoice> invoices = invoiceService.GetInvoices(currentAccount.account_name);
           
            return View(invoices);
        }

        public ActionResult GetInvoiceDetails(string invoiceId)
        {
            Account currentAccount = Session["account"] as Account;
            List<InvoiceDetail> invoicesDetails = invoiceService.GetInvoicesDetail(currentAccount.account_name, invoiceId);
            return View(invoicesDetails);
        }
    }
}