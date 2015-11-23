using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Routing;
using MvcTemplate.Components.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ViewLocationExpanderTests
    {
        #region Method: ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<String> viewLocations)

        [Fact]
        public void ExpandViewLocations_Area_ReturnsAreaLocations()
        {
            RouteData routeData = new RouteData();
            routeData.Values.Add("area", "Test");

            ActionContext actionContext = new ActionContext(new DefaultHttpContext(), routeData, new ActionDescriptor());
            ViewLocationExpanderContext context = new ViewLocationExpanderContext(actionContext, "Index", false);
            actionContext.ActionDescriptor.RouteConstraints = new List<RouteDataActionConstraint>();

            IEnumerable<String> expected = new[] { "/Views/{2}/Shared/{0}.cshtml", "/Views/{2}/{1}/{0}.cshtml", "/Views/Shared/{0}.cshtml" };
            IEnumerable<String> actual = new ViewLocationExpander().ExpandViewLocations(context, null);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExpandViewLocations_ReturnsViewLocations()
        {
            ActionContext actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            ViewLocationExpanderContext context = new ViewLocationExpanderContext(actionContext, "Index", false);
            actionContext.ActionDescriptor.RouteConstraints = new List<RouteDataActionConstraint>();

            IEnumerable<String> expected = new[] { "/Views/{1}/{0}.cshtml", "/Views/Shared/{0}.cshtml" };
            IEnumerable<String> actual = new ViewLocationExpander().ExpandViewLocations(context, null);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: PopulateValues(ViewLocationExpanderContext context)

        [Fact]
        public void PopulateValues_DoesNothing()
        {
            new ViewLocationExpander().PopulateValues(null);
        }

        #endregion
    }
}
