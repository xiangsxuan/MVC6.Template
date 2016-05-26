using Microsoft.AspNetCore.Http;
using MvcTemplate.Components.Logging;
using System;
using System.Threading.Tasks;

namespace MvcTemplate.Components.Mvc
{
    public class ExceptionFilterMiddleware
    {
        private ILogger Logger { get; }
        private RequestDelegate Next { get; }

        public ExceptionFilterMiddleware(RequestDelegate next, ILogger logger)
        {
            Logger = logger;
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
                Logger.Log(exception);

                throw;
            }
        }
    }
}
