using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class AccountController : Controller
    {
        private AccountService accountService = new AccountService();
        // GET: Account
        public ActionResult ShowProfile()
        {
            Account currentAccount = Session["account"] as Account;
            UserProfile userProfile = accountService.GetProfileByAccountName(currentAccount.account_name);
            ViewBag.err = TempData["err"];
            ViewBag.success = TempData["success"];
            return View(userProfile);
        }

        [HttpPost]
        public ActionResult UpdateMainProfile(string accountName, string firstName, string lastName, string email, string address, string phone)
        {
            bool isChanged = accountService.UpdateMainProfile(accountName, firstName, lastName, email, address, phone);
            if (isChanged == false)
                TempData["err"] = "account name không khớp";

            TempData["success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("ShowProfile");
        }

        [HttpPost]
        public ActionResult UpdatePassword(string accountName, string currentPassword, string newPassword, string confirmPassword)
        {
            bool isChanged = accountService.UpdatePassword(accountName, currentPassword, newPassword, confirmPassword);
            if (isChanged == false)
                TempData["err"] = "username sai hoặc password không khớp";

            TempData["success"] = "Cập nhật mật khẩu thành công!";
            return RedirectToAction("ShowProfile");
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool registrationSuccess = accountService.RegisterUser(model);

                if (registrationSuccess)
                {
                    ViewBag.ThongBao = "Đăng ký thành công";
                    return RedirectToAction("LogIn");
                }
                else
                {
                    ViewBag.error = accountService.GetRegistrationError();
                }
            }
            else
            {
                ViewBag.error = "Vui lòng điền đầy đủ thông tin.";
            }

            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                int authenticationSuccess = accountService.AuthenticateUser(model);

                if (authenticationSuccess == 1)
                {
                    Account acc = accountService.getUserByUsername(model.accountname);
                    Session["account"] = acc;
                    return RedirectToAction("Index", "Home");
                }
                else if (authenticationSuccess == 2)
                {
                    //Admin
                    Account acc = accountService.getUserByUsername(model.accountname);
                    Session["account"] = acc;
                    return RedirectToAction("Index", "Admin_Hompage");
                }
                else if (authenticationSuccess == 3)
                {
                    //staff
                    Account acc = accountService.getUserByUsername(model.accountname);
                    Session["account"] = acc;
                    return RedirectToAction("Index", "Staff");
                }
                else
                {
                    ViewBag.error = accountService.GetLoginError();
                }
            }
            else
            {
                ViewBag.error = "Vui lòng điền đầy đủ thông tin.";
            }

            return View();
        }
       
    }
}