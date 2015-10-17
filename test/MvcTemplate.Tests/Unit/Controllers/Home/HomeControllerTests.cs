using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class HomeControllerTests : ControllerTests
    {
        private HomeController controller;
        private IAccountService service;

        public HomeControllerTests()
        {
            service = Substitute.For<IAccountService>();
            controller = Substitute.ForPartsOf<HomeController>(service);

            ReturnCurrentAccountId(controller, "Test");
        }

        #region Method: Index()

        [Fact]
        public void Index_NotActive_RedirectsToLogout()
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

            ViewDataDictionary actual = (controller.Index() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Error()

        [Fact]
        public void Error_ReturnsEmptyView()
        {
            ViewDataDictionary actual = (controller.Error() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: NotFound()

        [Fact]
        public void NotFound_ReturnsEmptyView()
        {
            ViewDataDictionary actual = (controller.NotFound() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Unauthorized()

        [Fact]
        public void Unauthorized_NotActive_RedirectsToLogout()
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

            ViewDataDictionary actual = (controller.Unauthorized() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion
    }
}
