using Microsoft.AspNet.Mvc.Filters;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Security;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger Logger { get; }

        public ExceptionFilter(ILogger logger)
        {
            Logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            while (exception.InnerException != null)
                exception = exception.InnerException;

            String message = $"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}";

            Logger.Log(filterContext.HttpContext.User.Identity.Id(), message);
        }
    }
}
