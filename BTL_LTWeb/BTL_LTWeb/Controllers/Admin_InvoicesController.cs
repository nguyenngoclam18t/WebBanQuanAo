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
    public class Admin_InvoicesController : Controller
    {
        // GET: Invoices
        public ActionResult ViewInvoice()
        {
            
            BaseService QL = new BaseService();
            List<Invoice> list = QL.db.Invoices.OrderByDescending(t=>t.invoice_date).ToList();
            return View(list);
        }
        public void changeStatus(string id,string status)
        {
            BaseService QL = new BaseService();
            Invoice I = QL.db.Invoices.FirstOrDefault(i => i.invoice_id.CompareTo(id) == 0);
            I.status = status;
            QL.db.SaveChanges();
        }
        public ActionResult Success(String id)
        {
            changeStatus(id, "Success");
            return RedirectToAction("ViewInvoice");
        }
        public ActionResult Cancel(String id)
        {
            changeStatus(id, "Cancel");
            return RedirectToAction("ViewInvoice");
        }

    }
}