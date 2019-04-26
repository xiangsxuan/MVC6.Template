﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Renting.Components.Security;
using NSubstitute;
using System.Security.Claims;
using Xunit;

namespace Renting.Components.Mvc.Tests
{
    public class AuthorizationFilterTests
    {
        private ResourceExecutingContext context;
        private IAuthorization authorization;
        private AuthorizationFilter filter;

        public AuthorizationFilterTests()
        {
            ActionContext action = new ActionContext(Substitute.For<HttpContext>(), new RouteData(), new ActionDescriptor());
            context = new ResourceExecutingContext(action, new IFilterMetadata[0], new IValueProviderFactory[0]);
            authorization = Substitute.For<IAuthorization>();
            filter = new AuthorizationFilter(authorization);
        }

        #region OnResourceExecuting(ResourceExecutingContext context)

        [Fact]
        public void OnResourceExecuting_NotAuthenticated_SetsNullResult()
        {
            context.HttpContext.User.Identity.IsAuthenticated.Returns(false);

            filter.OnResourceExecuting(context);

            Assert.Null(context.Result);
        }

        [Fact]
        public void OnResourceExecuting_NotAuthorized_ReturnsNotFoundView()
        {
            context.HttpContext.User.Identity.IsAuthenticated.Returns(true);

            filter.OnResourceExecuting(context);

            ViewResult actual = context.Result as ViewResult;

            Assert.Equal("~/Views/Home/NotFound.cshtml", actual.ViewName);
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        public void OnResourceExecuting_IsAuthorized_SetsNullResult()
        {
            context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Returns(new Claim(ClaimTypes.NameIdentifier, "11000"));
            context.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            context.RouteData.Values["controller"] = "Controller";
            context.RouteData.Values["action"] = "Action";
            context.RouteData.Values["area"] = "Area";

            authorization.IsGrantedFor(11000, "Area", "Controller", "Action").Returns(true);

            filter.OnResourceExecuting(context);

            Assert.Null(context.Result);
        }

        #endregion
    }
}
