using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.OptionsModel;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ViewEngineTests
    {
        private ViewEngine engine;

        public ViewEngineTests()
        {
            IOptions<RazorViewEngineOptions> engineOptions = Substitute.For<IOptions<RazorViewEngineOptions>>();
            engineOptions.Options.Returns(new RazorViewEngineOptions());

            engine = new ViewEngine
            (
                Substitute.For<IRazorPageFactory>(),
                Substitute.For<IRazorViewFactory>(),
                engineOptions,
                Substitute.For<IViewLocationCache>()
            );
        }

        #region Constructor: ViewEngine(IRazorPageFactory pageFactory, IRazorViewFactory viewFactory, IOptions<RazorViewEngineOptions> optionsAccessor, IViewLocationCache viewLocationCache)

        [Fact]
        public void ViewEngine_SetsAreaViewLocationFormats()
        {
            IEnumerable<String> expected = new[] { "/Views/{2}/{1}/{0}.cshtml", "/Views/Shared/{0}.cshtml" };
            IEnumerable<String> actual = engine.AreaViewLocationFormats;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ViewEngine_SetsViewLocationFormats()
        {
            IEnumerable<String> expected = new[] { "/Views/{1}/{0}.cshtml", "/Views/Shared/{0}.cshtml" };
            IEnumerable<String> actual = engine.ViewLocationFormats;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
