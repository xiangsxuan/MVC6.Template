using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class AjaxOnlyAttributeTests
    {
        #region IsValidForRequest(RouteContext context, ActionDescriptor action)

        [Fact]
        public void IsValidForRequest_NullHeaders_ReturnsFalse()
        {
            RouteContext context = new RouteContext(Substitute.For<HttpContext>());
            context.HttpContext.Request.Headers.Returns(null as IHeaderDictionary);

            Assert.False(new AjaxOnlyAttribute().IsValidForRequest(context, null));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("XMLHttpRequest", true)]
        public void IsValidForRequest_Ajax(String header, Boolean isValid)
        {
            RouteContext context = new RouteContext(Substitute.For<HttpContext>());
            context.HttpContext.Request.Headers["X-Requested-With"].Returns(new StringValues(header));

            Boolean actual = new AjaxOnlyAttribute().IsValidForRequest(context, null);
            Boolean expected = isValid;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
