using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mail;
using MvcTemplate.Controllers;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.Administration.Accounts.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class AuthControllerTests : ControllerTests
    {
        private AccountRegisterView accountRegister;
        private AccountRecoveryView accountRecovery;
        private AccountResetView accountReset;
        private AccountLoginView accountLogin;
        private IAccountValidator validator;
        private AuthController controller;
        private IAccountService service;
        private IMailClient mailClient;

        public AuthControllerTests()
        {
            mailClient = Substitute.For<IMailClient>();
            service = Substitute.For<IAccountService>();
            validator = Substitute.For<IAccountValidator>();
            controller = Substitute.ForPartsOf<AuthController>(validator, service, mailClient);
            controller.ActionContext.HttpContext = Substitute.For<HttpContext>();
            controller.TempData = Substitute.For<ITempDataDictionary>();
            controller.Url = Substitute.For<IUrlHelper>();

            accountRegister = ObjectFactory.CreateAccountRegisterView();
            accountRecovery = ObjectFactory.CreateAccountRecoveryView();
            accountReset = ObjectFactory.CreateAccountResetView();
            accountLogin = ObjectFactory.CreateAccountLoginView();
        }

        #region Method: Register()

        [Fact]
        public void Register_IsLoggedIn_RedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.Register();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Register_ReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);

            ViewDataDictionary actual = (controller.Register() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Register(AccountRegisterView account)

        [Fact]
        public void Register_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Register");
        }

        [Fact]
        public void Register_IsLoggenIn_RedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.Register(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Register_CanNotRegister_ReturnsSameView()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRegister(accountRegister).Returns(false);

            Object actual = (controller.Register(accountRegister) as ViewResult).ViewData.Model;
            Object expected = accountRegister;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Register_Account()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRegister(accountRegister).Returns(true);

            controller.Register(accountRegister);

            service.Received().Register(accountRegister);
        }

        [Fact]
        public void Register_AddsRegistrationMessage()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRegister(accountRegister).Returns(true);

            controller.Register(accountRegister);

            Alert actual = controller.Alerts.Single();

            Assert.Equal(AlertType.Success, actual.Type);
            Assert.Equal(Messages.SuccessfulRegistration, actual.Message);
            Assert.Equal(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
        }

        [Fact]
        public void Register_RedirectsToLogin()
        {
            validator.CanRegister(accountRegister).Returns(true);
            service.IsLoggedIn(controller.User).Returns(false);

            RedirectToActionResult actual = controller.Register(accountRegister) as RedirectToActionResult;

            Assert.Equal("Login", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        #endregion

        #region Method: Recover()

        [Fact]
        public void Recover_IsLoggedIn_RedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.Recover();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Recover_ReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);

            ViewDataDictionary actual = (controller.Recover() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Recover(AccountRecoveryView account)

        [Fact]
        public void Recover_Post_IsLoggedIn_RedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            validator.CanRecover(accountRecovery).Returns(true);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.Recover(null).Result;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Recover_CanNotRecover_ReturnsSameView()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(false);

            Object actual = (controller.Recover(accountRecovery).Result as ViewResult).ViewData.Model;
            Object expected = accountRecovery;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Recover_Account()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);

            controller.Recover(accountRecovery).Wait();

            service.Received().Recover(accountRecovery);
        }

        [Fact]
        public void Recover_SendsRecoveryInformation()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns("TestToken");

            controller.Recover(accountRecovery).Wait();

            String url = controller.Url.Action("Reset", "Auth", new { token = "TestToken" }, controller.Request.Scheme);
            String body = String.Format(Messages.RecoveryEmailBody, url);
            String subject = Messages.RecoveryEmailSubject;
            String email = accountRecovery.Email;

            mailClient.Received().SendAsync(email, subject, body);
        }

        [Fact]
        public void Recover_NullToken_DoesNotSendRecoveryInformation()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns(null as String);

            controller.Recover(accountRecovery).Wait();

            mailClient.DidNotReceive().SendAsync(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>());
        }

        [Fact]
        public void Recover_AddsRecoveryMessage()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns("RecoveryToken");

            controller.Recover(accountRecovery).Wait();

            Alert actual = controller.Alerts.Single();

            Assert.Equal(Messages.RecoveryInformation, actual.Message);
            Assert.Equal(AlertType.Info, actual.Type);
            Assert.Equal(0, actual.FadeoutAfter);
        }

        [Fact]
        public void Recover_RedirectsToLogin()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanRecover(accountRecovery).Returns(true);
            service.Recover(accountRecovery).Returns("RecoveryToken");

            RedirectToActionResult actual = controller.Recover(accountRecovery).Result as RedirectToActionResult;

            Assert.Equal("Login", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        #endregion

        #region Method: Reset(String token)

        [Fact]
        public void Reset_IsLoggedIn_RedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.Reset("");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Reset_CanNotReset_RedirectsToRecover()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(Arg.Any<AccountResetView>()).Returns(false);

            RedirectToActionResult actual = controller.Reset("Token") as RedirectToActionResult;

            Assert.Equal("Recover", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Reset_ReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(Arg.Any<AccountResetView>()).Returns(true);

            ViewDataDictionary actual = (controller.Reset("") as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Reset(AccountResetView account)

        [Fact]
        public void Reset_Post_IsLoggedIn_RedirectsToDefault()
        {
            service.IsLoggedIn(controller.User).Returns(true);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.Reset(accountReset);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Reset_Post_CanNotReset_RedirectsToRecover()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(false);

            RedirectToActionResult actual = controller.Reset(accountReset) as RedirectToActionResult;

            Assert.Equal("Recover", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Reset_Account()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(true);

            controller.Reset(accountReset);

            service.Received().Reset(accountReset);
        }

        [Fact]
        public void Reset_AddsResetMessage()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(true);

            controller.Reset(accountReset);

            Alert actual = controller.Alerts.Single();

            Assert.Equal(AlertType.Success, actual.Type);
            Assert.Equal(Messages.SuccessfulReset, actual.Message);
            Assert.Equal(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
        }

        [Fact]
        public void Reset_RedirectsToLogin()
        {
            service.IsLoggedIn(controller.User).Returns(false);
            validator.CanReset(accountReset).Returns(true);

            RedirectToActionResult actual = controller.Reset(accountReset) as RedirectToActionResult;

            Assert.Equal("Login", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        #endregion

        #region Method: Login(String returnUrl)

        [Fact]
        public void Login_IsLoggedIn_RedirectsToUrl()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
            controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

            Object expected = controller.RedirectToLocal("/");
            Object actual = controller.Login("/");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Login_NotLoggedIn_ReturnsEmptyView()
        {
            service.IsLoggedIn(controller.User).Returns(false);

            ViewDataDictionary actual = (controller.Login("/") as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Login(AccountLoginView account, String returnUrl)

        [Fact]
        public void Login_Post_IsLoggedIn_RedirectsToUrl()
        {
            service.IsLoggedIn(controller.User).Returns(true);
            controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
            controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

            Object expected = controller.RedirectToLocal("/");
            Object actual = controller.Login(null, "/");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Login_CanNotLogin_ReturnsSameView()
        {
            validator.CanLogin(accountLogin).Returns(false);

            Object actual = (controller.Login(accountLogin, null) as ViewResult).ViewData.Model;
            Object expected = accountLogin;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Login_Account()
        {
            validator.CanLogin(accountLogin).Returns(true);
            controller.When(sub => sub.RedirectToLocal(null)).DoNotCallBase();
            controller.RedirectToLocal(null).Returns(new RedirectResult("/"));

            controller.Login(accountLogin, null);

            service.Received().Login(controller.HttpContext.Authentication, accountLogin.Username);
        }

        [Fact]
        public void Login_RedirectsToUrl()
        {
            validator.CanLogin(accountLogin).Returns(true);
            controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
            controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

            Object actual = controller.Login(accountLogin, "/");
            Object expected = controller.RedirectToLocal("/");

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Fact]
        public void Logout_Account()
        {
            controller.Logout();

            service.Received().Logout(controller.HttpContext.Authentication);
        }

        [Fact]
        public void Logout_RedirectsToLogin()
        {
            RedirectToActionResult actual = controller.Logout();

            Assert.Equal("Login", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        #endregion
    }
}
