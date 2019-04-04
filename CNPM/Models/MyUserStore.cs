using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CNPM.Models
{
    public class MyUserStore : IUserStore<user>, IUserPasswordStore<user>, 
        IUserSecurityStampStore<user>, IUserLockoutStore<user, string>,
        IUserTwoFactorStore<user, string>//,IUserLoginStore<user>
    {
        UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(new TMSdbEntities());
        public MyUserStore()
        {
        }
        public Task CreateAsync(user user)
        {
            var context = userStore.Context as TMSdbEntities;
            user.password = user.PasswordHash;
            context.users.Add(user);
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }
        public Task DeleteAsync(user user)
        {
            var context = userStore.Context as TMSdbEntities;
            context.users.Remove(user);
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }
        public async Task<user> FindByIdAsync(int userId)
        {
            var context = userStore.Context as TMSdbEntities;
            var re = await context.users.Where(u => u.id == userId).FirstOrDefaultAsync();
            return re == null ? null : re as user;
        }
        public async Task<user> FindByNameAsync(string userName)
        {
            var context = userStore.Context as TMSdbEntities;
            var re = await context.users.Where(u => u.email.ToLower() == userName.ToLower()).FirstOrDefaultAsync();
            return re == null ? null : re as user;
        }
        public Task UpdateAsync(user user)
        {
            var context = userStore.Context as TMSdbEntities;
            context.users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }
        public void Dispose()
        {
            userStore.Dispose();
        }
        public Task<string> GetPasswordHashAsync(user user)
        {
            return Task.FromResult(user.password);
        }

        public Task<bool> HasPasswordAsync(user user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.HasPasswordAsync(identityUser);
            SetTableUser(user, identityUser);
            return task;
        }
        public Task SetPasswordHashAsync(user user, string passwordHash)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetPasswordHashAsync(identityUser, passwordHash);
            SetTableUser(user, identityUser);
            return task;
        }
        public Task<string> GetSecurityStampAsync(user user)
        {
            //var identityUser = ToIdentityUser(user);
            //var task = userStore.GetSecurityStampAsync(identityUser);
            //SetTableUser(user, identityUser);
            //return task;
            return Task.FromResult(user.SecurityStamp);
        }
        public Task SetSecurityStampAsync(user user, string stamp)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetSecurityStampAsync(identityUser, stamp);
            SetTableUser(user, identityUser);
            return task;
        }
        private static void SetTableUser(user user, IdentityUser identityUser)
        {
            user.PasswordHash = identityUser.PasswordHash;
            //user.SecurityStamp = identityUser.SecurityStamp;
            user.email = identityUser.Email;
        }
        private IdentityUser ToIdentityUser(user user)
        {
            return new IdentityUser
            {
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,
                Email = user.email
            };
        }

        public async Task<user> FindByIdAsync(string userId)
        {
            var context = userStore.Context as TMSdbEntities;
            var re = await context.users.Where(u => u.id.ToString() == userId).FirstOrDefaultAsync();
            return re == null ? null : re as user;
        }

        //public async Task AddLoginAsync(user user, UserLoginInfo login)
        //{
        //    var context = userStore.Context as ABIExam_dbEntities;
        //    context.UserLoginExternals.Add(new UserLoginExternal()
        //    {
        //        UserId = user.Id,
        //        LoginProvider = login.LoginProvider,
        //        ProviderKey = login.ProviderKey
        //    });
        //    await context.SaveChangesAsync();
        //}

        //public async Task RemoveLoginAsync(user user, UserLoginInfo login)
        //{
        //    var context = userStore.Context as ABIExam_dbEntities;
        //    var r = await context.UserLoginExternals.Where(l => 
        //        l.UserId == user.Id &&
        //        l.LoginProvider == login.LoginProvider &&
        //        l.ProviderKey == login.ProviderKey
        //    ).FirstOrDefaultAsync();
        //    if (r != null)
        //        context.UserLoginExternals.Remove(r);
        //    await context.SaveChangesAsync();
        //}

        //public async Task<IList<UserLoginInfo>> GetLoginsAsync(user user)
        //{
        //    var context = userStore.Context as ABIExam_dbEntities;
        //    return await context.UserLoginExternals.Where(l => l.UserId == user.Id).Select(e => e.ToUserLoginInfo()).ToListAsync();
        //}

        //public async Task<user> FindAsync(UserLoginInfo login)
        //{
        //    var context = userStore.Context as ABIExam_dbEntities;
        //    var user = from x in context.UserLoginExternals
        //               where x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey
        //               join y in context.users on x.UserId equals y.Id
        //               select y;
        //    return await user.FirstOrDefaultAsync();
        //}

        #region logout
        public Task<DateTimeOffset> GetLockoutEndDateAsync(user user)
        {
            return Task.FromResult(DateTimeOffset.Now);
            //throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(user user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult(DateTimeOffset.Now);
            //throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(user user)
        {
            return Task.FromResult(0);
            //throw new NotImplementedException();
        }

        public async Task ResetAccessFailedCountAsync(user user)
        {
            return;
            //throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(user user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(user user)
        {
            return Task.FromResult(false);
        }

        public async Task SetLockoutEnabledAsync(user user, bool enabled)
        {
            return;
            //throw new NotImplementedException();
        }

        public async Task SetTwoFactorEnabledAsync(user user, bool enabled)
        {
            return;
            //throw new NotImplementedException();
        }

        public async Task<bool> GetTwoFactorEnabledAsync(user user)
        {
            return false;
            //throw new NotImplementedException();
        }
        #endregion
    }
}