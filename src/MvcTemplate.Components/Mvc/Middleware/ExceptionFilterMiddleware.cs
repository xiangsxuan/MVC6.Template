using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Logging;
using System;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class ExceptionFilterMiddleware
    {
        private RequestDelegate Next { get; }

        public ExceptionFilterMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception exception)
            {
                context.RequestServices.GetRequiredService<ILogger>().Log(exception);

                throw;
            }
        }
    }
}
