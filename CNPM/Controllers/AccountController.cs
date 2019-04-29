using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace CNPM.Controllers
{
    public class AccountController : Controller
    {
        #region attributes
        private AppSignInManager _signInManager;
        private AppUserManager _userManager;
        private TMSdbEntities db = new TMSdbEntities();
        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public AccountController()
        {
        }

        public AccountController(AppUserManager userManager)
        {
            this._userManager = userManager;
        }

        public AccountController(AppUserManager userManager, AppSignInManager signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        #endregion

        public ActionResult Login(string returnUrl)

        {
            return View(new LoginViewModel
            {
                returnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel request)
        {
            if (ModelState.IsValid)
            {
                // check is admin here
                //var db = new TMSdbEntities();
                
                var user = UserManager.Find(request.Email, request.Password);
                if (user == null)
                {
                    //ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                    ViewBag.Valid = "Invalid";
                    return View(request);
                }

                var role = "";
                var teacher = db.teachers.Where(t => t.user_id == user.id).FirstOrDefault();
                if(teacher != null)
                {
                    role = "teacher";
                }
                var student = db.students.Where(s => s.user_id == user.id).FirstOrDefault();
                if(student != null)
                {
                    role = "student";
                }

                var result = await SignInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        
                            if (role == "teacher") return RedirectToAction("Index", "Teacher");
                            else return RedirectToAction("Index", "Student");
                           
                        //return RedirectToLocal(request.returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = request.returnUrl, RememberMe = request.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                        return View(request);
                }
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}