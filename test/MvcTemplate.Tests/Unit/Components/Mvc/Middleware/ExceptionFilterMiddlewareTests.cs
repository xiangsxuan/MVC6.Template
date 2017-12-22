using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ExceptionFilterMiddlewareTests
    {
        #region Invoke(HttpContext context)

        [Fact]
        public void Invoke_LogsAndRethrows()
        {
            Exception exception = new Exception();
            ILogger logger = Substitute.For<ILogger>();
            HttpContext context = Substitute.For<HttpContext>();
            RequestDelegate next = Substitute.For<RequestDelegate>();
            context.RequestServices.GetService<ILogger>().Returns(logger);
            ExceptionFilterMiddleware middleware = new ExceptionFilterMiddleware(next);

            next.When(sub => sub(context)).Do(info => throw exception);

            Exception actual = Assert.Throws<AggregateException>(() => middleware.Invoke(context).Wait()).InnerException;
            Exception expected = exception;

            logger.Received().Log(exception);
            Assert.Same(expected, actual);
        }

        #endregion
    }
}
