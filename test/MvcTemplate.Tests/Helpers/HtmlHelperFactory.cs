using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;
using NSubstitute;
using System;

namespace MvcTemplate.Tests
{
    public static class HtmlHelperFactory
    {
        public static IHtmlHelper CreateHtmlHelper()
        {
            return CreateHtmlHelper<Object>(null);
        }
        public static IHtmlHelper<T> CreateHtmlHelper<T>(T model)
        {
            IHtmlHelper<T> html = Substitute.For<IHtmlHelper<T>>();

            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.RouteData = new RouteData();
            html.ViewContext.HttpContext = HttpContextFactory.CreateHttpContext();

            return html;
        }
    }
}