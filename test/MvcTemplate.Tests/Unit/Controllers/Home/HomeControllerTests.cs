using Microsoft.AspNet.Mvc;
using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class HomeControllerTests : AControllerTests
    {
        private HomeController controller;
        private IAccountService service;

        public HomeControllerTests()
        {
            service = Substitute.For<IAccountService>();
            controller = Substitute.ForPartsOf<HomeController>(service);

            ReturnsCurrentAccountId(controller, "Test");
        }

        #region Method: Index()

        [Fact]
        public void Index_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToActionResult actual = controller.Index() as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Index_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object model = (controller.Index() as ViewResult).ViewData.Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Error()

        [Fact]
        public void Error_ReturnsEmptyView()
        {
            Object model = (controller.Error() as ViewResult).ViewData.Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: NotFound()

        [Fact]
        public void NotFound_ReturnsEmptyView()
        {
            Object model = (controller.NotFound() as ViewResult).ViewData.Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: Unauthorized()

        [Fact]
        public void Unauthorized_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToActionResult actual = controller.Unauthorized() as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Unauthorized_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object model = (controller.Unauthorized() as ViewResult).ViewData.Model;

            Assert.Null(model);
        }

        #endregion
    }
}
