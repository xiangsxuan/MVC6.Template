using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Primitives;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class AjaxOnlyAttributeTests
    {
        #region IsValidForRequest(RouteContext routeContext, ActionDescriptor action)

        [Fact]
        public void IsValidForRequest_NullHeader_ReturnsFalse()
        {
            RouteContext context = new RouteContext(Substitute.For<HttpContext>());
            context.HttpContext.Request.Headers.Returns(null as IHeaderDictionary);

            Assert.False(new AjaxOnlyAttribute().IsValidForRequest(context, null));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("XMLHttpRequest", true)]
        public void IsValidForRequest_Ajax(String headerValue, Boolean expected)
        {
            RouteContext context = new RouteContext(Substitute.For<HttpContext>());
            context.HttpContext.Request.Headers["X-Requested-With"].Returns(new StringValues(headerValue));

            Boolean actual = new AjaxOnlyAttribute().IsValidForRequest(context, null);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
