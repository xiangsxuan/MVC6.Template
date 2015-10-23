using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Routing;
using MvcTemplate.Components.Alerts;
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
    public class ProfileControllerTests : ControllerTests
    {
        private ProfileDeleteView profileDelete;
        private ProfileController controller;
        private ProfileEditView profileEdit;
        private IAccountValidator validator;
        private IAccountService service;

        public ProfileControllerTests()
        {
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            profileDelete = ObjectFactory.CreateProfileDeleteView();
            profileEdit = ObjectFactory.CreateProfileEditView();

            controller = Substitute.ForPartsOf<ProfileController>(validator, service);
            controller.ActionContext.RouteData = new RouteData();
            ReturnCurrentAccountId(controller, "Test");
        }

        #region Method: Edit()

        [Fact]
        public void Edit_NotActive_RedirectsToLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            Object expected = RedirectToAction(controller, "Logout", "Auth");
            Object actual = controller.Edit();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Edit_ReturnsProfileView()
        {
            service.Get<ProfileEditView>(controller.CurrentAccountId).Returns(profileEdit);
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object actual = (controller.Edit() as ViewResult).ViewData.Model;
            Object expected = profileEdit;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Edit(ProfileEditView profile)

        [Fact]
        public void Edit_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Edit");
        }

        [Fact]
        public void Edit_Post_NotActive_RedirectsToLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            Object expected = RedirectToAction(controller, "Logout", "Auth");
            Object actual = controller.Edit(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Edit_CanNotEdit_ReturnsSameView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(false);

            Object actual = (controller.Edit(profileEdit) as ViewResult).ViewData.Model;
            Object expected = profileEdit;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Edit_Profile()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(true);

            controller.Edit(profileEdit);

            service.Received().Edit(profileEdit);
        }

        [Fact]
        public void Edit_AddsUpdatedMessage()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(true);

            controller.Edit(profileEdit);

            Alert actual = controller.Alerts.Single();

            Assert.Equal(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.Equal(Messages.ProfileUpdated, actual.Message);
            Assert.Equal(AlertType.Success, actual.Type);
        }

        [Fact]
        public void Edit_RedirectsToEdit()
        {
            validator.CanEdit(profileEdit).Returns(true);
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object expected = RedirectToAction(controller, "Edit");
            Object actual = controller.Edit(profileEdit);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Delete()

        [Fact]
        public void Delete_NotActive_RedirectsToLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            Object expected = RedirectToAction(controller, "Logout", "Auth");
            Object actual = controller.Delete();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Delete_AddsDisclaimerMessage()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            controller.Delete();

            Alert actual = controller.Alerts.Single();

            Assert.Equal(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal(0, actual.FadeoutAfter);
        }

        [Fact]
        public void Delete_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            ViewDataDictionary actual = (controller.Delete() as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: DeleteConfirmed(ProfileDeleteView profile)

        [Fact]
        public void DeleteConfirmed_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "DeleteConfirmed");
        }

        [Fact]
        public void DeleteConfirmed_NotActive_RedirectsToLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            Object expected = RedirectToAction(controller, "Logout", "Auth");
            Object actual = controller.DeleteConfirmed(profileDelete);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void DeleteConfirmed_CanNotDelete_AddsDisclaimerMessage()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(false);

            controller.DeleteConfirmed(profileDelete);

            Alert actual = controller.Alerts.Single();

            Assert.Equal(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal(0, actual.FadeoutAfter);
        }

        [Fact]
        public void DeleteConfirmed_CanNotDelete_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(false);

            ViewDataDictionary actual = (controller.DeleteConfirmed(profileDelete) as ViewResult).ViewData;

            Assert.Null(actual.Model);
        }

        [Fact]
        public void DeleteConfirmed_DeletesProfile()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(true);

            controller.DeleteConfirmed(profileDelete);

            service.Received().Delete(controller.CurrentAccountId);
        }

        [Fact]
        public void DeleteConfirmed_RedirectsToAuthLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(true);

            Object expected = RedirectToAction(controller, "Logout", "Auth");
            Object actual = controller.DeleteConfirmed(profileDelete);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
