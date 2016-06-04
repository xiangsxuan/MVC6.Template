using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class GlobalizationFilter : IResourceFilter
    {
        private IGlobalizationProvider Globalization { get; }

        public GlobalizationFilter(IGlobalizationProvider globalization)
        {
            Globalization = globalization;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Globalization.CurrentLanguage = Globalization[context.RouteData.Values["language"] as String];
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
