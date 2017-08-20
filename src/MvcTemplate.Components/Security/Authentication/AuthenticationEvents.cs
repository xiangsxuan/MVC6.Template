using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Security
{
    public class AuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            RouteData route = context.HttpContext.GetRouteData();
            ActionContext action = new ActionContext(context.HttpContext, route, new ActionDescriptor());
            context.RedirectUri = new UrlHelper(action).Action("Login", "Auth", new { area = "", returnUrl = context.Request.Path });

            return base.RedirectToLogin(context);
        }
    }
}
