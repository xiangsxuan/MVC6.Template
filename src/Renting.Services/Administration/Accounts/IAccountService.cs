﻿using Microsoft.AspNetCore.Http;
using Renting.Objects;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Renting.Services
{
    public interface IAccountService : IService
    {
        TView Get<TView>(Int32 id) where TView : BaseView;
        IQueryable<AccountView> GetViews();

        Boolean IsLoggedIn(IPrincipal user);
        Boolean IsActive(Int32 id);

        String Recover(AccountRecoveryView view);
        void Reset(AccountResetView view);

        void Create(AccountCreateView view);
        void Edit(AccountEditView view);

        void Edit(ClaimsPrincipal user, ProfileEditView view);
        void Delete(Int32 id);

        Task Login(HttpContext context, String username);
        Task Logout(HttpContext context);
    }
}
