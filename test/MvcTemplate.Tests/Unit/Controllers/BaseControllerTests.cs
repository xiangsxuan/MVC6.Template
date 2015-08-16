using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class BaseControllerTests : IDisposable
    {
        private BaseController controller;

        public BaseControllerTests()
        {
            HttpContext httpContext = HttpContextFactory.CreateHttpContext();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            controller = Substitute.ForPartsOf<BaseController>();

            controller.Url = Substitute.For<IUrlHelper>();
            controller.ActionContext.RouteData = new RouteData();
            controller.ActionContext.HttpContext = HttpContextFactory.CreateHttpContext();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Property: CurrentAccountId

        [Fact]
        public void CurrentAccountId_GetsCurrentIdentityName()
        {
            String expected = controller.User.Identity.Name;
            String actual = controller.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: BaseController()

        [Fact]
        public void BaseController_SetsAuthorizationProviderFromFactory()
        {
            IAuthorizationProvider actual = controller.AuthorizationProvider;
            IAuthorizationProvider expected = Authorization.Provider;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseController_CreatesEmptyAlertsContainer()
        {
            Assert.Empty(controller.Alerts);
        }

        #endregion

        #region Method: NotEmptyView(Object model)

        [Fact]
        public void NotEmptyView_RedirectsToNotFoundIfModelIsNull()
        {
            controller.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            controller.RedirectToNotFound().Returns(new RedirectToActionResult(null, null, null));

            ActionResult expected = controller.RedirectToNotFound();
            ActionResult actual = controller.NotEmptyView(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void NotEmptyView_ReturnsEmptyView()
        {
            Object expected = new Object();
            Object actual = (controller.NotEmptyView(expected) as ViewResult).ViewData.Model;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: RedirectToLocal(String url)

        [Fact]
        public void RedirectToLocal_RedirectsToDefaultIfUrlIsNotLocal()
        {
            controller.Url.IsLocalUrl("www.test.com").Returns(false);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToActionResult(null, null, null));

            ActionResult actual = controller.RedirectToLocal("www.test.com");
            ActionResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectToLocal_RedirectsToLocalIfUrlIsLocal()
        {
            controller.Url.IsLocalUrl("/").Returns(true);

            String actual = (controller.RedirectToLocal("/") as RedirectResult).Url;
            String expected = "/";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Fact]
        public void RedirectToDefault_RedirectsToDefault()
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
        public void RedirectToNotFound_RedirectsToNotFound()
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
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
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
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            controller.IsAuthorizedFor("Action").Returns(false);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToActionResult(null, null, null));

            RedirectToActionResult actual = controller.RedirectIfAuthorized("Action");
            RedirectToActionResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RedirectsToActionIfAuthorized()
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
        public void RedirectIfAuthorized_RouteValues_RedirectsToDefaultIfNotAuthorizedWithSpecifiedRoute()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);

            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToActionResult(null, null, null));

            RedirectToActionResult actual = controller.RedirectIfAuthorized("Action", new { controller = "Control", area = "Area" });
            RedirectToActionResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RouteValues_RedirectsToDefaultIfNotAuthorized()
        {
            String areaRoute = controller.RouteData.Values["area"] as String;
            String controllerRoute = controller.RouteData.Values["controller"] as String;
            controller.IsAuthorizedFor(areaRoute, controllerRoute, "Action").Returns(false);

            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToActionResult(null, null, null));

            RedirectToActionResult actual = controller.RedirectIfAuthorized("Action", new { id = "Id" });
            RedirectToActionResult expected = controller.RedirectToDefault();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_RouteValues_RedirectsToActionIfAuthorized()
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
        public void IsAuthorizedFor_ReturnsTrueThenAuthorized()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(true);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.True(controller.IsAuthorizedFor("Action"));
        }

        [Fact]
        public void IsAuthorizedFor_ReturnsFalseThenNotAuthorized()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.False(controller.IsAuthorizedFor("Action"));
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Fact]
        public void IsAuthorizedFor_OnNullAuthorizationProviderReturnsTrue()
        {
            Authorization.Provider = null;
            controller = Substitute.ForPartsOf<BaseController>();

            Assert.Null(controller.AuthorizationProvider);
            Assert.True(controller.IsAuthorizedFor(null, null, null));
        }

        [Fact]
        public void IsAuthorizedFor_ReturnsAuthorizationProviderResult()
        {
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "AR", "CO", "AC").Returns(true);

            Assert.True(controller.IsAuthorizedFor("AR", "CO", "AC"));
        }

        #endregion
    }
}
