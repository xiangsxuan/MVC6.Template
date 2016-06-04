using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class GlobalizationFilterTests
    {
        #region OnResourceExecuting(ResourceExecutingContext context)

        [Fact]
        public void OnActionExecuting_SetsCurrentLanguage()
        {
            ActionContext action = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            ResourceExecutingContext context = new ResourceExecutingContext(action, new IFilterMetadata[0]);
            IGlobalizationProvider provider = Substitute.For<IGlobalizationProvider>();
            context.RouteData.Values["language"] = "lt";
            provider["lt"].Returns(new Language());

            new GlobalizationFilter(provider).OnResourceExecuting(context);

            Language actual = provider.CurrentLanguage;
            Language expected = provider["lt"];

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
