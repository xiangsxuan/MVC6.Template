using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Routing;
using MvcTemplate.Controllers.Administration;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
{
    public class AccountsControllerTests : ControllerTests
    {
        private AccountCreateView accountCreate;
        private AccountsController controller;
        private IAccountValidator validator;
        private AccountEditView accountEdit;
        private IAccountService service;
        private AccountView account;

        public AccountsControllerTests()
        {
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            accountCreate = ObjectFactory.CreateAccountCreateView();
            accountEdit = ObjectFactory.CreateAccountEditView();
            account = ObjectFactory.CreateAccountView();

            controller = Substitute.ForPartsOf<AccountsController>(validator, service);
            controller.ActionContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Fact]
        public void Index_ReturnsAccountViews()
        {
            service.GetViews().Returns(new AccountView[0].AsQueryable());

            Object actual = controller.Index().ViewData.Model;
            Object expected = service.GetViews();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Fact]
        public void Create_ReturnsEmptyView()
        {
            ViewDataDictionary actual = controller.Create().ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Create(AccountCreateView account)

        [Fact]
        public void Create_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Create");
        }

        [Fact]
        public void Create_CanNotCreate_ReturnsSameView()
        {
            validator.CanCreate(accountCreate).Returns(false);

            Object actual = (controller.Create(accountCreate) as ViewResult).ViewData.Model;
            Object expected = accountCreate;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Create_Account()
        {
            validator.CanCreate(accountCreate).Returns(true);

            controller.Create(accountCreate);

            service.Received().Create(accountCreate);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            validator.CanCreate(accountCreate).Returns(true);

            Object expected = RedirectIfAuthorized(controller, "Index");
            Object actual = controller.Create(accountCreate);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Fact]
        public void Details_ReturnsNotEmptyView()
        {
            service.Get<AccountView>(account.Id).Returns(account);

            Object expected = NotEmptyView(controller, account);
            Object actual = controller.Details(account.Id);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Fact]
        public void Edit_ReturnsNotEmptyView()
        {
            service.Get<AccountEditView>(accountEdit.Id).Returns(accountEdit);

            Object expected = NotEmptyView(controller, accountEdit);
            Object actual = controller.Edit(accountEdit.Id);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView account)

        [Fact]
        public void Edit_CanNotEdit_ReturnsSameView()
        {
            validator.CanEdit(accountEdit).Returns(false);

            Object actual = (controller.Edit(accountEdit) as ViewResult).ViewData.Model;
            Object expected = accountEdit;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Edit_Account()
        {
            validator.CanEdit(accountEdit).Returns(true);

            controller.Edit(accountEdit);

            service.Received().Edit(accountEdit);
        }

        [Fact]
        public void Edit_RedirectsToIndex()
        {
            validator.CanEdit(accountEdit).Returns(true);

            Object expected = RedirectIfAuthorized(controller, "Index");
            Object actual = controller.Edit(accountEdit);

            Assert.Same(expected, actual);
        }

        #endregion
    }
}
