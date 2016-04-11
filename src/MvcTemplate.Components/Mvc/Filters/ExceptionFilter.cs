using Microsoft.AspNet.Mvc.Filters;
using MvcTemplate.Components.Logging;

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
            Logger.Log(filterContext.Exception);
        }
    }
}
