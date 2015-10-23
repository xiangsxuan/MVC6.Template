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
        private String controllerName;
        private String actionName;
        private String areaName;

        public BaseControllerTests()
        {
            HttpContext httpContext = Substitute.For<HttpContext>();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            controller = Substitute.ForPartsOf<BaseController>();

            controller.Url = Substitute.For<IUrlHelper>();
            controller.ActionContext.RouteData = new RouteData();
            controller.TempData = Substitute.For<ITempDataDictionary>();
            controller.ActionContext.HttpContext = Substitute.For<HttpContext>();

            controllerName = controller.RouteData.Values["controller"] as String;
            actionName = controller.RouteData.Values["action"] as String;
            areaName = controller.RouteData.Values["area"] as String;
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
            Assert.Equal("Home", actual.ControllerName);
            Assert.Equal("Index", actual.ActionName);
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

        #region Method: RedirectToAction(String actionName, String controllerName, Object routeValues)

        [Fact]
        public void RedirectToAction_Action_Controller_Route_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectToAction("Action", "Controller", new { id = "Id" });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectToAction_Action_NullController_NullRoute_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(true);

            RedirectToActionResult expected = controller.RedirectToAction("Action", null, null);
            RedirectToActionResult actual = controller.RedirectToAction("Action", null, null);

            Assert.Equal(expected.ControllerName, actual.ControllerName);
            Assert.Equal(expected.ActionName, actual.ActionName);
            Assert.Equal("", actual.RouteValues["area"]);
            Assert.Single(actual.RouteValues);
        }

        [Fact]
        public void RedirectToAction_Action_Controller_NullRoute_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(true);

            RedirectToActionResult expected = controller.RedirectToAction("Action", "Controller", null);
            RedirectToActionResult actual = controller.RedirectToAction("Action", "Controller", null);

            Assert.Equal(expected.ControllerName, actual.ControllerName);
            Assert.Equal(expected.ActionName, actual.ActionName);
            Assert.Equal("", actual.RouteValues["area"]);
            Assert.Single(actual.RouteValues);
        }

        [Fact]
        public void RedirectToAction_Action_Controller_Route_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", "Controller", "Area").Returns(true);

            RedirectToActionResult expected = controller.RedirectToAction("Action", "Controller", new { area = "Area", id = "Id" });
            RedirectToActionResult actual = controller.RedirectToAction("Action", "Controller", new { area = "Area", id = "Id" });

            Assert.Equal(expected.ControllerName, actual.ControllerName);
            Assert.Equal(expected.ActionName, actual.ActionName);
            Assert.Equal("", actual.RouteValues["area"]);
            Assert.Single(actual.RouteValues);
        }

        #endregion

        #region Method: IsAuthorizedFor(String action, String controller, String area)

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
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "Area", "Controller", "Action").Returns(true);

            Assert.True(controller.IsAuthorizedFor("Action", "Controller", "Area"));
        }

        #endregion
    }
}
