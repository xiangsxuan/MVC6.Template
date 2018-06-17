using Microsoft.AspNetCore.Http;
using MvcTemplate.Components.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class SecureHeadersMiddlewareTests
    {
        #region Invoke(HttpContext context)

        [Fact]
        public async Task Invoke_AddsSecureHeaders()
        {
            HttpContext context = new DefaultHttpContext();

            await new SecureHeadersMiddleware(next => Task.CompletedTask).Invoke(context);

            IHeaderDictionary actual = context.Response.Headers;

            Assert.Equal("script-src 'self'; style-src 'self' 'unsafe-inline'", actual["Content-Security-Policy"]);
            Assert.Equal("1; mode=block", actual["X-XSS-Protection"]);
            Assert.Equal("nosniff", actual["X-Content-Type-Options"]);
            Assert.Equal("deny", actual["X-Frame-Options"]);
            Assert.Equal(4, actual.Keys.Count);
        }

        #endregion
    }
}
