using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using MvcTemplate.Components.Extensions;
using MvcTemplate.Components.Security;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class AuthorizationFilter : IResourceFilter
    {
        private IAuthorizationProvider Authorization { get; }

        public AuthorizationFilter(IAuthorizationProvider authorization)
        {
            Authorization = authorization;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                return;

            Int32? accountId = context.HttpContext.User.Id();
            String area = context.RouteData.Values["area"] as String;
            String action = context.RouteData.Values["action"] as String;
            String controller = context.RouteData.Values["controller"] as String;

            if (!IsAuthorizedFor(accountId, action, controller, area))
                context.Result = RedirectToUnauthorized(context);
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        private Boolean IsAuthorizedFor(Int32? accountId, String action, String controller, String area)
        {
            if (Authorization == null)
                return true;

            return Authorization.IsAuthorizedFor(accountId, area, controller, action);
        }
        private IActionResult RedirectToUnauthorized(ActionContext context)
        {
            RouteValueDictionary route = new RouteValueDictionary();
            route["language"] = context.RouteData.Values["language"];
            route["action"] = "Unauthorized";
            route["controller"] = "Home";
            route["area"] = "";

            return new RedirectToRouteResult(route);
        }
    }
}
