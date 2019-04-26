﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renting.Components.Mail;
using Renting.Objects;
using Renting.Resources;
using Renting.Services;
using Renting.Validators;
using System;
using System.Threading.Tasks;

namespace Renting.Controllers
{
    [AllowAnonymous]
    public class AuthController : ValidatedController<IAccountValidator, IAccountService>
    {
        public IMailClient MailClient { get; }

        public AuthController(IAccountValidator validator, IAccountService service, IMailClient mailClient)
            : base(validator, service)
        {
            MailClient = mailClient;
        }

        [HttpGet]
        public ActionResult Recover()
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Recover(AccountRecoveryView account)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            if (!Validator.CanRecover(account))
                return View(account);

            if (Service.Recover(account) is String token)
            {
                String url = Url.Action("Reset", "Auth", new { token }, Request.Scheme);

                await MailClient.SendAsync(account.Email,
                    Message.For<AccountView>("RecoveryEmailSubject"),
                    Message.For<AccountView>("RecoveryEmailBody", url));
            }

            Alerts.AddInfo(Message.For<AccountView>("RecoveryInformation"));

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Reset(String token)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            if (!Validator.CanReset(new AccountResetView { Token = token }))
                return RedirectToAction("Recover");

            return View();
        }

        [HttpPost]
        public ActionResult Reset(AccountResetView account)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToDefault();

            if (!Validator.CanReset(account))
                return RedirectToAction("Recover");

            Service.Reset(account);

            Alerts.AddSuccess(Message.For<AccountView>("SuccessfulReset"), 4000);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login(String returnUrl)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToLocal(returnUrl);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginView account, String returnUrl)
        {
            if (Service.IsLoggedIn(User))
                return RedirectToLocal(returnUrl);

            if (!Validator.CanLogin(account))
                return View(account);

            await Service.Login(HttpContext, account.Username);

            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public async Task<RedirectToActionResult> Logout()
        {
            await Service.Logout(HttpContext);

            return RedirectToAction("Login");
        }
    }
}
