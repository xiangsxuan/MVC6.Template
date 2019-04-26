﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Renting.Components.Extensions;
using Renting.Components.Security;
using Renting.Data.Core;
using Renting.Objects;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Renting.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IHasher Hasher { get; }

        public AccountService(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            Hasher = hasher;
        }

        public TView Get<TView>(Int32 id) where TView : BaseView
        {
            return UnitOfWork.GetAs<Account, TView>(id);
        }
        public IQueryable<AccountView> GetViews()
        {
            return UnitOfWork
                .Select<Account>()
                .To<AccountView>()
                .OrderByDescending(account => account.Id);
        }

        public Boolean IsLoggedIn(IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }
        public Boolean IsActive(Int32 id)
        {
            return UnitOfWork.Select<Account>().Any(account => account.Id == id && !account.IsLocked);
        }

        public String Recover(AccountRecoveryView view)
        {
            Account account = UnitOfWork.Select<Account>().SingleOrDefault(model => model.Email.ToLower() == view.Email.ToLower());
            if (account == null)
                return null;

            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);
            account.RecoveryToken = Guid.NewGuid().ToString();

            UnitOfWork.Update(account);
            UnitOfWork.Commit();

            return account.RecoveryToken;
        }
        public void Reset(AccountResetView view)
        {
            Account account = UnitOfWork.Select<Account>().Single(model => model.RecoveryToken == view.Token);
            account.Passhash = Hasher.HashPassword(view.NewPassword);
            account.RecoveryTokenExpirationDate = null;
            account.RecoveryToken = null;

            UnitOfWork.Update(account);
            UnitOfWork.Commit();
        }

        public void Create(AccountCreateView view)
        {
            Account account = UnitOfWork.To<Account>(view);
            account.Passhash = Hasher.HashPassword(view.Password);
            account.Email = view.Email.ToLower();

            UnitOfWork.Insert(account);
            UnitOfWork.Commit();
        }
        public void Edit(AccountEditView view)
        {
            Account account = UnitOfWork.Get<Account>(view.Id);
            account.IsLocked = view.IsLocked;
            account.RoleId = view.RoleId;

            UnitOfWork.Update(account);
            UnitOfWork.Commit();
        }

        public void Edit(ClaimsPrincipal user, ProfileEditView view)
        {
            Account account = UnitOfWork.Get<Account>(CurrentAccountId);
            account.Email = view.Email.ToLower();
            account.Username = view.Username;

            if (!String.IsNullOrWhiteSpace(view.NewPassword))
                account.Passhash = Hasher.HashPassword(view.NewPassword);

            UnitOfWork.Update(account);
            UnitOfWork.Commit();

            user.UpdateClaim(ClaimTypes.Name, account.Username);
            user.UpdateClaim(ClaimTypes.Email, account.Email);
        }
        public void Delete(Int32 id)
        {
            UnitOfWork.Delete<Account>(id);
            UnitOfWork.Commit();
        }

        public async Task Login(HttpContext context, String username)
        {
            Account account = UnitOfWork.Select<Account>().Single(model => model.Username.ToLower() == username.ToLower());

            await context.SignInAsync("Cookies", new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Email, account.Email)
            }, "Password")));
        }
        public async Task Logout(HttpContext context)
        {
            await context.SignOutAsync("Cookies");
        }
    }
}
