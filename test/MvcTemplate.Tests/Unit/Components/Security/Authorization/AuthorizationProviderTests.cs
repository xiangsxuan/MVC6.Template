using MvcTemplate.Components.Security;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class AuthorizationProviderTests
    {
        private AuthorizationProvider provider;

        public AuthorizationProviderTests()
        {
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly());

            TearDownData();
        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [InlineData("")]
        [InlineData(null)]
        [Theory(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            String actual = Assert.Throws<Exception>(() => provider.IsAuthorizedFor(null, null, "Authorized", "Test")).Message;
            String expected = "'AuthorizedController' does not have 'Test' action.";

            Assert.Equal(expected, actual);
        }

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
        public void IsAuthorizedFor_AuthorizesActionWithoutAuthorizeAsAttribute()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "NotAuthorizedAs");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "NotAuthorizedAs"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionWithoutAuthorizeAsAttribute()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "NotAuthorizedAs"));
        }

        [Fact(Skip = "EF not supporting navigation properties")]
        public void IsAuthorizedFor_AuthorizesActionAsItself()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedAsItself");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsItself"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionAsItself()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsItself"));
        }

        [Fact(Skip = "EF not supporting navigation properties")]
        public void IsAuthorizedFor_AuthorizesActionAsOtherAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionAsOtherAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Test", "AuthorizedAsAction");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsAction"));
        }

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
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

        [Fact(Skip = "EF not supporting navigation properties")]
        public void IsAuthorizedFor_CachesAccountPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "Action");
            TearDownData();

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "Action"));
        }

        #endregion

        #region Method: Refresh()

        [Fact(Skip = "EF not supporting navigation properties")]
        public void Refresh_RefreshesPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "Action");
            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));

            TearDownData();

            provider.Refresh();

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        #endregion

        #region Test helpers

        private Account CreateAccountWithPrivilegeFor(String area, String controller, String action, Boolean isLocked = false)
        {
            using (TestingContext context = new TestingContext())
            {
                Account account = ObjectFactory.CreateAccount();
                Role role = ObjectFactory.CreateRole();
                account.IsLocked = isLocked;
                account.RoleId = role.Id;
                account.Role = role;

                role.RolePrivileges = new List<RolePrivilege>();
                RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
                Privilege privilege = ObjectFactory.CreatePrivilege();
                rolePrivilege.PrivilegeId = privilege.Id;
                rolePrivilege.Privilege = privilege;
                rolePrivilege.RoleId = role.Id;
                rolePrivilege.Role = role;

                privilege.Controller = controller;
                privilege.Action = action;
                privilege.Area = area;

                role.RolePrivileges.Add(rolePrivilege);

                context.Set<Role>().Add(account.Role);
                context.Set<Account>().Add(account);
                context.SaveChanges();

                context.Set<Privilege>().Add(rolePrivilege.Privilege);
                context.Set<RolePrivilege>().Add(rolePrivilege);
                context.SaveChanges();

                provider.Refresh();

                return account;
            }
        }

        private void TearDownData()
        {
            using (TestingContext context = new TestingContext())
            {
                context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
                context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
                context.Set<Account>().RemoveRange(context.Set<Account>());
                context.Set<Role>().RemoveRange(context.Set<Role>());
                context.SaveChanges();
            }
        }

        #endregion
    }
}
