using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Security
{
    public class AuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(CookieRedirectContext context)
        {
            RouteData route = context.HttpContext.GetRouteData();
            ActionContext action = new ActionContext(context.HttpContext, route, new ActionDescriptor());
            IUrlHelper url = context.Request.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(action);

            context.RedirectUri = url.Action("Login", "Auth", new { area = "", returnUrl = context.Request.Path }, context.Request.Scheme);

            return base.RedirectToLogin(context);
        }
    }
}
