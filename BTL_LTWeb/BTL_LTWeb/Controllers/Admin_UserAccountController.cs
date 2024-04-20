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
    public class Admin_UserAccountController : Controller
    {
        // GET: Admin_UserAccount
        int pageSize = 10;
        public ActionResult ViewUserAccount(int?page,string nameAccount="")
        {
            BaseService QL = new BaseService();
            int pageNumber = (page ?? 0);
            var accountQuery = from Account in QL.db.Accounts
                          join User in QL.db.Users on Account.users_id equals User.users_id
                          where Account.role == "User"
                          select new
                          {
                              accName= Account.account_name,
                              email=User.email,
                              phone= User.phone,
                              adr= User.adr,
                              last_name= User.last_name,
                              first_name= User.first_name,
                              isdeleted= Account.isdeleted
                          };
        
            var ListItem = accountQuery.ToList();
            int totalCount = ListItem.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));
            List<Admin_UserAcount> list = new List<Admin_UserAcount>();
            ListItem = accountQuery.OrderBy(a => a.accName).Where(a=>a.accName.Contains( nameAccount)).Skip((pageNumber-1) * pageSize).Take(pageSize).ToList();
            foreach (var item in ListItem)
            {
                Admin_UserAcount tmp = new Admin_UserAcount();
                tmp.account_name= item.accName;
                tmp.email= item.email;
                tmp.phone=item.phone;
                tmp.adr=item.adr;
                tmp.last_name= item.last_name;
                tmp.first_name= item.first_name;
                tmp.isDeleted = (item.isdeleted??false);
                list.Add(tmp);
            }
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;
            //ViewBag.NameProduct = nameProduct;
            return View(list);
        }

        [HttpPost]
        public ActionResult ViewUserAccount(string nameAccount = "", int? page = 1)
        {
            return RedirectToAction("ViewUserAccount", new { page = page, nameAccount = nameAccount });
        }
        public ActionResult Block(string account_name)
        {
            BaseService QL = new BaseService();
            Account A = QL.db.Accounts.FirstOrDefault(a => a.account_name == account_name);
            A.isdeleted = true;
            QL.db.SaveChanges();
            return RedirectToAction("ViewUserAccount");
        }
        public ActionResult UnBlock(string account_name)
        {
            BaseService QL = new BaseService();
            Account A = QL.db.Accounts.FirstOrDefault(a => a.account_name == account_name);
            A.isdeleted = false;
            QL.db.SaveChanges();
            return RedirectToAction("ViewUserAccount");
        }
    }
}