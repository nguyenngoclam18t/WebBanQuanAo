using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using BTL_LTWeb.Models;
using PagedList;
using BTL_LTWeb.Services;
namespace BTL_LTWeb.Controllers
{
    public class Admin_HompageController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
        SqlConnection con;
        // GET: Admin
        public void getAmoutDashboard()
        {
            try
            {
                con = new SqlConnection(constring);
                con.Open();
                string query = " select Count(*) from Account where role='User'";
                SqlCommand sqlcmd = new SqlCommand(query, con);
                ViewBag.soluongUser = int.Parse(sqlcmd.ExecuteScalar().ToString());
                //
                query = "select count(*) from Invoices where status ='Pending'";
                sqlcmd = new SqlCommand(query, con);
                ViewBag.Pending = int.Parse(sqlcmd.ExecuteScalar().ToString());
                //
                query = "select count(*) from Product";
                sqlcmd = new SqlCommand(query, con);
                ViewBag.AmoutProduct = int.Parse(sqlcmd.ExecuteScalar().ToString());
                //
                query = "select ISNULL(SUM(total_amount),0) from Invoices where Status='Success'  and MONTH(invoice_date)=MONTH(GETDATE())";
                sqlcmd = new SqlCommand(query, con);
                ViewBag.AmoutSell = float.Parse(sqlcmd.ExecuteScalar().ToString());
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public ActionResult Index()
        {
            getAmoutDashboard();
            BaseService QL = new BaseService();
            List<Invoice> list = QL.db.Invoices.Take(5).OrderByDescending(t=>t.invoice_date).ToList();
            return View(list);
        }

       
    }
}