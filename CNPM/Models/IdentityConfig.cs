using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CNPM.Models
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new CNPM.Models.TMSdbEntities());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            //app.CreatePerOwinContext<RoleManager<IdentityRole>>((options, context) =>
            //    new RoleManager<IdentityRole>(
            //        new RoleStore<IdentityRole>(context.Get<MyDbContext>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }

    public class AppSignInManager : SignInManager<user, string>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
           : base(userManager, authenticationManager)
        {
        }

        public async override Task<ClaimsIdentity> CreateUserIdentityAsync(user user)
        {
            var appUserManager = (AppUserManager)UserManager;
            var userIdentity = await appUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }
}