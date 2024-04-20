using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;
namespace BTL_LTWeb.Services
{
    public class AccountService : BaseService
    {
        public UserProfile GetProfileByAccountName(string accountName)
        {
            UserProfile userProfile = db.Accounts
                .Where(account => account.account_name == accountName)
                .Join(db.Users,
                      account => account.users_id,
                      user => user.users_id,
                      (account, user) => new UserProfile
                      {
                          UserId = user.users_id,
                          FirstName = user.first_name,
                          LastName = user.last_name,
                          Address = user.adr,
                          Phone = user.phone,
                          Email = user.email,
                          AccountName = account.account_name,
                          Role = account.role,
                      })
                .FirstOrDefault();

            return userProfile;
        }

        public bool UpdateMainProfile(string accountName, string firstName, string lastName, string email, string address, string phone)
        {
            var account = db.Accounts.SingleOrDefault(a => a.account_name == accountName);

            if (account != null)
            {
                var user = db.Users.SingleOrDefault(u => u.users_id == account.users_id);

                if (user != null)
                {
                    user.first_name = firstName;
                    user.last_name = lastName;
                    user.email = email;
                    user.adr = address;
                    user.phone = phone;

                    db.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public bool UpdatePassword(string accountName, string currentPassword, string newPassword, string confirmPassword)
        {
            // Retrieve the account based on the accountName
            var account = db.Accounts.SingleOrDefault(a => a.account_name == accountName);

            if (account != null)
            {
                // Check if the current password matches the stored password
                if (account.account_password == currentPassword)
                {
                    // Check if the new password and confirm password match
                    if (newPassword == confirmPassword)
                    {
                        // Update the password and save changes
                        account.account_password = newPassword;
                        db.SaveChanges();
                        return true; // Password updated successfully
                    }
                    else
                    {
                        // Handle case where new password and confirm password do not match
                        return false; // Passwords do not match
                    }
                }
                else
                {
                    // Handle case where current password does not match stored password
                    return false; // Current password is incorrect
                }
            }

            // Handle case where account with the given accountName is not found
            return false; // Account not found
        }
        public string RegistrationError { get; private set; }
        public string LoginError { get; private set; }
        public int AuthenticateUser(LoginViewModel model)
        {
            var check = db.Accounts
                    .Where(s => s.account_name.Equals(model.accountname) && s.account_password.Equals(model.password))
                    .FirstOrDefault();
            if (check == null)
            {
                LoginError = "Tên tài khoản hoặc mật khẩu không đúng !";
                return 0;
            }
            else if (check.isdeleted == true)
            {
                LoginError = "Tài khoản của bạn đã bị khóa";
                return 0;
            }
            if (string.IsNullOrEmpty(model.accountname) || string.IsNullOrEmpty(model.password))
            {
                LoginError = "Vui lòng điền đầy đủ thông tin";
                return 0;
            }
            else
            {
                HttpContext.Current.Session["Tên tài khoản"] = model.accountname;

                if (check.role == "User")
                {
                    return 1;
                }
                else if (check.role == "Admin")
                {
                    return 2;
                }
                else if (check.role == "Staff")
                {
                    return 3;
                }
            }

            return 0;
        }
        public bool RegisterUser(RegisterViewModel model)
        {
            if (model == null)
                return false;

            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password) || string.IsNullOrEmpty(model.email))
            {
                RegistrationError = "Vui lòng điền đầy đủ thông tin";
                return false;
            }

            if (!IsValidEmail(model.email))
            {
                RegistrationError = "Địa chỉ email không hợp lệ";
                return false;
            }

            var checkUsername = db.Accounts.Any(a => a.account_name == model.username);
            if (checkUsername)
            {
                RegistrationError = "Account Name này đã được đăng ký";
                return false;
            }

            var check = db.Users.FirstOrDefault(s => s.email == model.email);
            if (check != null)
            {
                RegistrationError = "Tài khoản Email này đã được đăng kí.";
                return false;
            }

            db.Configuration.ValidateOnSaveEnabled = false;

            User user = new User
            {
                email = model.email
            };

            db.Users.Add(user);
            db.SaveChanges();

            int userId = user.users_id;

            Account account = new Account
            {
                account_name = model.email,
                account_password = model.password,
                role = "User",
                users_id = userId,
                isdeleted = false
            };
            db.Accounts.Add(account);
            db.SaveChanges();

            HttpContext.Current.Session["account"] = account;
            return true;
        }
        public string GetRegistrationError()
        {
            return RegistrationError;
        }
        public string GetLoginError()
        {
            return LoginError;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public Account getUserByUsername(string username)
        {
    
            return db.Accounts.First(t=>t.account_name== username);
        }
    }
}