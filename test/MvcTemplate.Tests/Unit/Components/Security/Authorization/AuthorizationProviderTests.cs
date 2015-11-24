using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Reflection;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class AuthorizationProviderTests
    {
        private AuthorizationProvider provider;
        private IServiceProvider serviceProvider;

        public AuthorizationProviderTests()
        {
            serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IUnitOfWork)).Returns(info => new UnitOfWork(new TestingContext()));

            using (TestingContext context = new TestingContext()) context.DropData();
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly(), serviceProvider);

        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        [Fact]
        public void IsAuthorizedFor_AuthorizesControllerByIgnoringCase()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "AUTHORIZED", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeControllerByIgnoringCase()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "AUTHORIZED", "Action"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void IsAuthorizedFor_AuthorizesControllerWithoutArea(String area)
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, area, "Authorized", "Action"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void IsAuthorizedFor_DoesNotAuthorizeControllerWithoutArea(String area)
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, area, "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesControllerWithArea()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeControllerWithArea()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedGetAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNamedGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedNamedGetAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNamedGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNotExistingAction()
        {
            Assert.True(provider.IsAuthorizedFor(null, null, "Authorized", "Test"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNonGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedPostAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNonGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNamedNonGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedNamedPostAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNamedNonGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesActionAsAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionAsAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "Action");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesActionAsOtherAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "InheritanceAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsOtherAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionAsOtherAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedAsOtherAction");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsOtherAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesEmptyAreaAsNull()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeEmptyAreaAsNull()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAuthorizedAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizedAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizedAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizedAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AllowAnonymousAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AllowUnauthorizedAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "InheritanceAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowAnonymous", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesInheritedAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowUnauthorized", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNotExistingAccount()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.False(provider.IsAuthorizedFor("Test", "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeLockedAccount()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action", isLocked: true);

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNullAccount()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "Action");

            Assert.False(provider.IsAuthorizedFor(null, null, "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesByIgnoringCase()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "area", "authorized", "action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeByIgnoringCase()
        {
            Account account = CreateAccountWithPrivilegeFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "area", "authorized", "action"));
        }

        [Fact]
        public void IsAuthorizedFor_CachesAccountPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "Action");
            using (TestingContext context = new TestingContext()) context.DropData();

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "Action"));
        }

        #endregion

        #region Method: Refresh()

        [Fact]
        public void Refresh_Privileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");
            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));

            using (TestingContext context = new TestingContext()) context.DropData();

            provider.Refresh();

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        #endregion

        #region Test helpers

        private Account CreateAccountWithPrivilegeFor(String area, String controller, String action, Boolean isLocked = false)
        {
            using (TestingContext context = new TestingContext())
            {
                RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
                Account account = ObjectFactory.CreateAccount();
                account.RoleId = rolePrivilege.RoleId;
                account.IsLocked = isLocked;

                rolePrivilege.Privilege.Controller = controller;
                rolePrivilege.Privilege.Action = action;
                rolePrivilege.Privilege.Area = area;

                context.Add(rolePrivilege.Privilege);
                context.Add(rolePrivilege);
                context.Add(account.Role);
                context.Add(account);

                context.SaveChanges();
            }

            provider.Refresh();
            
            return ObjectFactory.CreateAccount();
        }

        #endregion
    }
}