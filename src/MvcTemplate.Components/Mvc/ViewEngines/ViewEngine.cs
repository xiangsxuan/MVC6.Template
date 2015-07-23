using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class ViewEngine : RazorViewEngine
    {
        public override IEnumerable<String> AreaViewLocationFormats
        {
            get
            {
                return AreaViewFormats;
            }
        }
        public override IEnumerable<String> ViewLocationFormats
        {
            get
            {
                return ViewFormats;
            }
        }
        private IEnumerable<String> AreaViewFormats
        {
            get;
            set;
        }
        private IEnumerable<String> ViewFormats
        {
            get;
            set;
        }

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
