using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Renting.Components.Extensions;
using Renting.Components.Security;
using System;

namespace Renting.Components.Mvc
{
    public class AuthorizationFilter : IResourceFilter
    {
        private IAuthorization Authorization { get; }

        public AuthorizationFilter(IAuthorization authorization)
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

            if (Authorization?.IsGrantedFor(accountId, area, controller, action) == false)
                context.Result = new ViewResult
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ViewName = "~/Views/Home/NotFound.cshtml"
                };
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
