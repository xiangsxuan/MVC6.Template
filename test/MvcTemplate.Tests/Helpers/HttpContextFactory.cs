using Microsoft.AspNet.Http;
using NSubstitute;
namespace MvcTemplate.Tests
{
    public static class HttpContextFactory
    {
        public static HttpContext CreateHttpContext()
        {
            return Substitute.For<HttpContext>();
        }
    }
}
