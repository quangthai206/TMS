using CNPM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class AppUserManager : UserManager<user>
    {
        public AppUserManager(MyUserStore store) : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new MyUserStore());
                //new UserStore<TableUser>(context.Get<GameasaEntities>()));

            // optionally configure your manager
            // ...

            return manager;
        }
    }
}