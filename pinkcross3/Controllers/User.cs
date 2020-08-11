using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace pinkcross3.Controllers
{
    public class User : Controller
    {
        private const string LOGIN_SQL =
        @"SELECT * FROM [User] 
            WHERE Username_id = '{0}' 
              AND Password = HASHBYTES('SHA1', '{1}')";

        private const string REDIRECT_CNTR = "Home";
        private const string REDIRECT_ACTN = "Index";

        private const string LOGIN_VIEW = "Login";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View("Register");
        }
        public IActionResult RegisterDonorProfile()
        {
            return View("RegisterDonorProfile");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(Models.User rgs)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Register");
            }
            else
            {
                String insert = @"INSERT INTO [User](Username_id, Password, Role) VALUES ('{0}',HASHBYTES('SHA1','{1}'), 'Donor')";
                if (DBUtl.ExecSQL(insert, rgs.Username_id, rgs.Password) == 1)
                {
                    ViewData["Message"] = "Username ID and Password successfully created";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
            }
            return View("RegisterDonorProfile");
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterDonorProfile(Models.DonorProfile DP)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("RegisterDonorProfile");
            }
            else
            {
                String insert = @"INSERT INTO DonorProfile(DonorName, Donornric, CompanyName, DonorNumber, DonorAddress) VALUES ('{0}','{1}','{2}','{3}','{4}')";
                if (DBUtl.ExecSQL(insert, DP.DonorName, DP.Donornric, DP.CompanyName, DP.DonorNumber, DP.DonorAddress) == 1)
                {
                    ViewData["Message"] = "Register successfully";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
            }
            return View("Login");
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ForgetPassword(Models.ForgetPassword FP)
        {
            string Username_id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var sql = String.Format($"UPDATE Donors SET Password = HASHBYTES('SHA1', '{FP.NewPassword}') WHERE Username_id = '{Username_id}')");
            if (DBUtl.ExecSQL(sql, FP.NewPassword) == 1)
            {
                ViewData["Message"] = "Password successfully updated";
                ViewData["MsgType"] = "success";
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
            }
            return View("login");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Models.User user)
        {
            if (!AuthenticateUser(user.Username_id, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User ID or Password";
                ViewData["MsgType"] = "warning";
                Debug.WriteLine("Incorrect User ID or Password");
                return View(LOGIN_VIEW);
            }
            else
            {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal,
                   new AuthenticationProperties
                   {
                       IsPersistent = false
                   });

                if (TempData["returnUrl"] != null)
                {
                    Debug.WriteLine("returnUrl is not null");
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        Debug.WriteLine("Redirect to returnUrl" + returnUrl);
                        return Redirect(returnUrl);
                    }
                }

                Debug.WriteLine("We are going to redirect to Admin Index");
                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
        }

        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable user = DBUtl.GetTable(LOGIN_SQL, uid, pw);
            if (user.Rows.Count == 1)
            {
                principal =
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                           new Claim[]
                           {
                               new Claim(ClaimTypes.NameIdentifier, uid),
                               new Claim(ClaimTypes.Name, user.Rows[0]["Username_id"].ToString()),
                               new Claim(ClaimTypes.Role, user.Rows[0]["Role"].ToString())
                           }, "Basic"));
                return true;
            }
            return false;
        }
        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
        }
    }
}
