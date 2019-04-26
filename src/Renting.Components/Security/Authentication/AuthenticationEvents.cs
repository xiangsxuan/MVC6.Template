﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Renting.Components.Security
{
    public class AuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            ActionContext action = new ActionContext(context.HttpContext, context.HttpContext.GetRouteData(), new ActionDescriptor());
            context.RedirectUri = new UrlHelper(action).Action("Login", "Auth", new { area = "", returnUrl = context.Request.PathBase + context.Request.Path });

            return base.RedirectToLogin(context);
        }
    }
}
