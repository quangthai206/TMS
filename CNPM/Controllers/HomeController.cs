using CNPM.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CNPM.Controllers
{
    [Authorize]
    public class HomeController : Controller
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
        public HomeController()
        {
        }

        public HomeController(AppUserManager userManager)
        {
            this._userManager = userManager;
        }

        public HomeController(AppUserManager userManager, AppSignInManager signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task Add()
        //{
        //    var user = new user { email = "huyvietnguyen@gmail.com", name = "Student 3", create_at = DateTime.Now, active = true };
        //    var result = await UserManager.CreateAsync(user, "tms2019");
        //    if (result.Succeeded)
        //    {
        //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //    }
        //}
    }
}
