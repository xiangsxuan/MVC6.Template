using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class ViewEngine : RazorViewEngine
    {
        private IEnumerable<String> ViewFormats { get; }
        private IEnumerable<String> AreaViewFormats { get; }

        public override IEnumerable<String> ViewLocationFormats => ViewFormats;
        public override IEnumerable<String> AreaViewLocationFormats => AreaViewFormats;

        public ViewEngine(IRazorPageFactory pageFactory, IRazorViewFactory viewFactory, IOptions<RazorViewEngineOptions> optionsAccessor, IViewLocationCache viewLocationCache)
            : base(pageFactory, viewFactory, optionsAccessor, viewLocationCache)
        {
            AreaViewFormats = new[]
            {
                "/Views/{2}/{1}/{0}.cshtml",
                "/Views/Shared/{0}.cshtml"
            };
            ViewFormats = new[]
            {
                "/Views/{1}/{0}.cshtml",
                "/Views/Shared/{0}.cshtml"
            };
        }
    }
}
