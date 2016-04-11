using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Routing;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ExceptionFilterTests
    {
        #region Method: OnException(ExceptionContext filterContext)

        [Fact]
        public void OnException_Logs()
        {
            Exception exception = new Exception();
            ILogger logger = Substitute.For<ILogger>();
            ExceptionFilter filter = new ExceptionFilter(logger);
            ActionContext actionContext = new ActionContext
            {
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
                HttpContext = Substitute.For<HttpContext>()
            };

            ExceptionContext context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            context.Exception = exception;

            filter.OnException(context);

            logger.Received().Log(exception);
        }

        #endregion
    }
}
