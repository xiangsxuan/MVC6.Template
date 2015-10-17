using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Resources
{
    public class ResourceProviderTests
    {
        #region Static method: GetContentTitle(IDictionary<String, Object> values)

        [Fact]
        public void GetContentTitle_IsCaseInsensitive()
        {
            IDictionary<String, Object> values = new Dictionary<String, Object>();
            values["area"] = "administration";
            values["controller"] = "roles";
            values["action"] = "details";

            String expected = ContentTitles.AdministrationRolesDetails;
            String actual = ResourceProvider.GetContentTitle(values);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContentTitle_WithoutArea(String area)
        {
            IDictionary<String, Object> values = new Dictionary<String, Object>();
            values["controller"] = "profile";
            values["action"] = "edit";
            values["area"] = area;

            String actual = ResourceProvider.GetContentTitle(values);
            String expected = ContentTitles.ProfileEdit;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetContentTitle_NotFound_ReturnsNull()
        {
            IDictionary<String, Object> values = new Dictionary<String, Object>
            {
                ["controller"] = null,
                ["action"] = null,
                ["area"] = null
            };

            Assert.Null(ResourceProvider.GetContentTitle(values));
        }

        #endregion

        #region Static method: GetSiteMapTitle(String area, String controller, String action)

        [Fact]
        public void GetSiteMapTitle_IsCaseInsensitive()
        {
            String actual = ResourceProvider.GetSiteMapTitle("administration", "roles", "index");
            String expected = MvcTemplate.Resources.SiteMap.Titles.AdministrationRolesIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSiteMapTitle_WithoutControllerAndAction()
        {
            String actual = ResourceProvider.GetSiteMapTitle("administration", null, null);
            String expected = MvcTemplate.Resources.SiteMap.Titles.Administration;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSiteMapTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetSiteMapTitle("Test", "Test", "Test"));
        }

        #endregion

        #region Static method: GetPrivilegeAreaTitle(String area)

        [Fact]
        public void GetPrivilegeAreaTitle_IsCaseInsensitive()
        {
            String expected = MvcTemplate.Resources.Privilege.Area.Titles.Administration;
            String actual = ResourceProvider.GetPrivilegeAreaTitle("administration");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeAreaTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle("Test"));
        }

        [Fact]
        public void GetPrivilegeAreaTitle_NullArea_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeAreaTitle(null));
        }

        #endregion

        #region Static method: GetPrivilegeControllerTitle(String area, String controller)

        [Fact]
        public void GetPrivilegeControllerTitle_ReturnsTitle()
        {
            String expected = MvcTemplate.Resources.Privilege.Controller.Titles.AdministrationRoles;
            String actual = ResourceProvider.GetPrivilegeControllerTitle("Administration", "Roles");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeControllerTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeControllerTitle("", ""));
        }

        #endregion

        #region Static method: GetPrivilegeActionTitle(String area, String controller, String action)

        [Fact]
        public void GetPrivilegeActionTitle_ReturnsTitle()
        {
            String actual = ResourceProvider.GetPrivilegeActionTitle("administration", "accounts", "index");
            String expected = MvcTemplate.Resources.Privilege.Action.Titles.AdministrationAccountsIndex;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPrivilegeActionTitle_NotFound_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPrivilegeActionTitle("", "", ""));
        }

        #endregion

        #region Static method: GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)

        [Fact]
        public void GetPropertyTitle_NotMemberExpression_ReturnNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(view => view.ToString()));
        }

        [Fact]
        public void GetPropertyTitle_FromExpression()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Username);
            String expected = MvcTemplate.Resources.Views.Administration.Accounts.AccountView.Titles.Username;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_FromExpressionRelation()
        {
            String actual = ResourceProvider.GetPropertyTitle<AccountEditView, String>(account => account.RoleId);
            String expected = MvcTemplate.Resources.Views.Administration.Roles.RoleView.Titles.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_NotFoundExpression_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<AccountView, String>(account => account.Id));
        }

        [Fact]
        public void GetPropertyTitle_NotFoundType_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle<TestView, String>(test => test.Text));
        }

        #endregion

        #region Static method: GetPropertyTitle(Type view, String property)

        [Fact]
        public void GetPropertyTitle_IsCaseInsensitive()
        {
            String expected = MvcTemplate.Resources.Views.Administration.Accounts.AccountView.Titles.Username;
            String actual = ResourceProvider.GetPropertyTitle(typeof(AccountView), "username");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_FromRelation()
        {
            String expected = MvcTemplate.Resources.Views.Administration.Accounts.AccountView.Titles.Username;
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountUsername");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_FromMultipleRelations()
        {
            String expected = MvcTemplate.Resources.Views.Administration.Accounts.AccountView.Titles.Username;
            String actual = ResourceProvider.GetPropertyTitle(typeof(RoleView), "AccountRoleAccountUsername");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertyTitle_NotFoundProperty_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(AccountView), "Id"));
        }

        [Fact]
        public void GetPropertyTitle_NotFoundTypeProperty_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(TestView), "Title"));
        }

        [Fact]
        public void GetPropertyTitle_NullKey_ReturnsNull()
        {
            Assert.Null(ResourceProvider.GetPropertyTitle(typeof(RoleView), null));
        }

        #endregion
    }
}
