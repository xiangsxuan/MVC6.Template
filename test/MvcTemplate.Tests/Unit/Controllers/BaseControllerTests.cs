using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Routing;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class BaseControllerTests : ControllerTests, IDisposable
    {
        private BaseController controller;

        public BaseControllerTests()
        {
            HttpContext httpContext = Substitute.For<HttpContext>();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            controller = Substitute.ForPartsOf<BaseController>();

            controller.Url = Substitute.For<IUrlHelper>();
            controller.ActionContext.RouteData = new RouteData();
            controller.TempData = Substitute.For<ITempDataDictionary>();
            controller.ActionContext.HttpContext = Substitute.For<HttpContext>();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Property: CurrentAccountId

        [Fact]
        public void CurrentAccountId_ReturnsIdentityName()
        {
            String expected = controller.User.Identity.Name;
            String actual = controller.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: BaseController()

        [Fact]
        public void BaseController_SetsAuthorization()
        {
            IAuthorizationProvider actual = controller.AuthorizationProvider;
            IAuthorizationProvider expected = Authorization.Provider;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseController_CreatesEmptyAlerts()
        {
            Assert.Empty(controller.Alerts);
        }

        #endregion

        #region Method: NotEmptyView(Object model)

        [Fact]
        public void NotEmptyView_NullModel_RedirectsToNotFound()
        {
            Object expected = RedirectToNotFound(controller);
            Object actual = controller.NotEmptyView(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void NotEmptyView_ReturnsModelView()
        {
            Object expected = new Object();
            Object actual = (controller.NotEmptyView(expected) as ViewResult).ViewData.Model;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: RedirectToLocal(String url)

        [Fact]
        public void RedirectToLocal_NotLocalUrl_RedirectsToDefault()
        {
            controller.Url.IsLocalUrl("T").Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectToLocal("T");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectToLocal_IsLocalUrl_RedirectsToLocal()
        {
            controller.Url.IsLocalUrl("/").Returns(true);

            String actual = (controller.RedirectToLocal("/") as RedirectResult).Url;
            String expected = "/";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Fact]
        public void RedirectToDefault_Route()
        {
            RedirectToActionResult actual = controller.RedirectToDefault();

            Assert.Equal("", actual.RouteValues["area"]);
            Assert.Equal("", actual.ControllerName);
            Assert.Equal("", actual.ActionName);
            Assert.Single(actual.RouteValues);
        }

        #endregion

        #region Method: RedirectToNotFound()

        [Fact]
        public void RedirectToNotFound_Route()
        {
            RedirectToActionResult actual = controller.RedirectToNotFound();

            Assert.Equal("", actual.RouteValues["area"]);
            Assert.Equal("NotFound", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Single(actual.RouteValues);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Fact]
        public void RedirectToUnauthorized_Route()
        {
            RedirectToActionResult actual = controller.RedirectToUnauthorized();

            Assert.Equal("Unauthorized", actual.ActionName);
            Assert.Equal("", actual.RouteValues["area"]);
            Assert.Equal("Home", actual.ControllerName);
            Assert.Single(actual.RouteValues);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Fact]
        public void RedirectIfAuthorized_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Action").Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action").Returns(true);

            RedirectToActionResult actual = controller.RedirectIfAuthorized("Action");
            RedirectToActionResult expected = controller.RedirectToAction("Action");

            Assert.Equal(expected.RouteValues.Count, actual.RouteValues.Count);
            Assert.Equal(expected.ControllerName, actual.ControllerName);
            Assert.Equal(expected.ActionName, actual.ActionName);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action, Object routeValues)

        [Fact]
        public void RedirectIfAuthorized_SpecificRoute_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action", new { controller = "Control", area = "Area" });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_DefaultRoute_NotAuthorized_RedirectsToDefault()
        {
            String areaRoute = controller.RouteData.Values["area"] as String;
            String controllerRoute = controller.RouteData.Values["controller"] as String;
            controller.IsAuthorizedFor(areaRoute, controllerRoute, "Action").Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action", new { id = "Id" });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_Route_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Area", "Control", "Action").Returns(true);

            RedirectToActionResult actual = controller.RedirectIfAuthorized("Action", new { controller = "Control", area = "Area", id = "Id" });
            RedirectToActionResult expected = controller.RedirectToAction("Action", new { controller = "Control", area = "Area", id = "Id" });

            Assert.Equal(expected.RouteValues["area"], actual.RouteValues["area"]);
            Assert.Equal(expected.RouteValues["id"], actual.RouteValues["id"]);
            Assert.Equal(expected.RouteValues.Count, actual.RouteValues.Count);
            Assert.Equal(expected.ControllerName, actual.ControllerName);
            Assert.Equal(expected.ActionName, actual.ActionName);
        }

        #endregion

        #region Method: IsAuthorizedFor(String action)

        [Fact]
        public void IsAuthorizedFor_True()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(true);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.True(controller.IsAuthorizedFor("Action"));
        }

        [Fact]
        public void IsAuthorizedFor_False()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.False(controller.IsAuthorizedFor("Action"));
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Fact]
        public void IsAuthorizedFor_NullAuthorizationProvider_ReturnsTrue()
        {
            Authorization.Provider = null;
            controller = Substitute.ForPartsOf<BaseController>();

            Assert.Null(controller.AuthorizationProvider);
            Assert.True(controller.IsAuthorizedFor(null, null, null));
        }

        [Fact]
        public void IsAuthorizedFor_ReturnsAuthorizationResult()
        {
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "AR", "CO", "AC").Returns(true);

            Assert.True(controller.IsAuthorizedFor("AR", "CO", "AC"));
        }

        #endregion
    }
}
