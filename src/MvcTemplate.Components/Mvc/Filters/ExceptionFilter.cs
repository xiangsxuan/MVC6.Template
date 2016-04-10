using Microsoft.AspNet.Mvc.Filters;
using MvcTemplate.Components.Logging;
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

            Logger.Log($"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }
    }
}
