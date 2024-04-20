using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTL_LTWeb.Models;

namespace BTL_LTWeb.Services
{
    public class InvoiceService : BaseService
    {
        public List<Invoice> GetInvoices(string username)
        {
            List<Invoice> invoices = db.Invoices
            .Where(i => i.Account.account_name == username)
            .ToList();

            return invoices;
        }

        public List<InvoiceDetail> GetInvoicesDetail(string username, string invoiceId)
        {
            List<InvoiceDetail> invoicesDetail = db.InvoiceDetails
            .Where(id => id.Invoice.Account.account_name == username && id.invoice_id == invoiceId)
            .ToList();

            return invoicesDetail;
        }
    }
}