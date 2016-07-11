using Microsoft.AspNetCore.Http;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ExceptionFilterMiddlewareTests
    {
        private ExceptionFilterMiddleware middleware;
        private RequestDelegate next;
        private HttpContext context;
        private ILogger logger;

        public ExceptionFilterMiddlewareTests()
        {
            logger = Substitute.For<ILogger>();
            context = Substitute.For<HttpContext>();
            next = Substitute.For<RequestDelegate>();
            middleware = new ExceptionFilterMiddleware(next, logger);
        }

        #region Invoke(HttpContext context)

        [Fact]
        public async Task Invoke_NextAction()
        {
            await middleware.Invoke(context);

            await next.Received()(context);
        }

        [Fact]
        public void Invoke_LogsAndRethrows()
        {
            Exception exception = new Exception();
            next.When(sub => sub(context)).Do(info => { throw exception; });

            Exception actual = Assert.Throws<AggregateException>(() => middleware.Invoke(context).Wait()).InnerException;
            Exception expected = exception;

            logger.Received().Log(exception);
            Assert.Same(exception, actual);
        }

        #endregion
    }
}
