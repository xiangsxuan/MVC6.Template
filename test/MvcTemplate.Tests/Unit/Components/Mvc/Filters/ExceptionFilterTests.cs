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
        private ActionContext actionContext;
        private ExceptionFilter filter;
        private Exception exception;
        private ILogger logger;

        public ExceptionFilterTests()
        {
            exception = GenerateException();
            logger = Substitute.For<ILogger>();
            actionContext = new ActionContext();
            filter = new ExceptionFilter(logger);
            actionContext.RouteData = new RouteData();
            actionContext.ActionDescriptor = new ActionDescriptor();
            actionContext.HttpContext = Substitute.For<HttpContext>();
        }

        #region Method: OnException(ExceptionContext filterContext)

        [Fact]
        public void OnException_LogsException()
        {
            ExceptionContext context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            context.Exception = exception;

            filter.OnException(context);
            String expectedMessage = String.Format("{0}: {1}{2}{3}",
                exception.GetType(),
                exception.Message,
                Environment.NewLine,
                exception.StackTrace);

            logger.Received().Log(context.HttpContext.User.Identity.Name, expectedMessage);
        }

        [Fact]
        public void OnException_LogsInnerMostException()
        {
            ExceptionContext context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            context.Exception = new Exception("O", exception);

            filter.OnException(context);
            String expectedMessage = String.Format("{0}: {1}{2}{3}",
                context.Exception.InnerException.GetType(),
                context.Exception.InnerException.Message,
                Environment.NewLine,
                context.Exception.InnerException.StackTrace);

            logger.Received().Log(context.HttpContext.User.Identity.Name, expectedMessage);
        }

        #endregion

        #region Test helpers

        private Exception GenerateException()
        {
            try
            {
                return new Exception(((Object)null).ToString());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        #endregion
    }
}
