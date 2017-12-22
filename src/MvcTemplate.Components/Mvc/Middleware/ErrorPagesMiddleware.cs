using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using MvcTemplate.Resources.Shared;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class ErrorPagesMiddleware
    {
        private RequestDelegate Next { get; }

        public ErrorPagesMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch
            {
                if (context.Request.Headers != null && context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.StatusCode = 500;

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { status = "error", data = new { message = Strings.SystemError } }));
                }
                else
                {
                    Redirect(context, "Error", "Home", new { area = "" });
                }
            }
        }

        private void Redirect(HttpContext context, String action, String controller, Object values)
        {
            RouteData route = (context.Features[typeof(IRoutingFeature)] as IRoutingFeature)?.RouteData;
            IUrlHelper url = new UrlHelper(new ActionContext(context, route, new ActionDescriptor()));

            context.Response.Redirect(url.Action(action, controller, values));
        }
    }
}
