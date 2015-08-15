using Microsoft.AspNet.Mvc;
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
    public class ProfileControllerTests : AControllerTests
    {
        private ProfileDeleteView profileDelete;
        private ProfileController controller;
        private ProfileEditView profileEdit;
        private IAccountValidator validator;
        private IAccountService service;

        public ProfileControllerTests()
        {
            profileEdit = ObjectFactory.CreateProfileEditView(1);
            profileDelete = ObjectFactory.CreateProfileDeleteView(2);

            service = Substitute.For<IAccountService>();
            validator = Substitute.For<IAccountValidator>();
            controller = Substitute.ForPartsOf<ProfileController>(validator, service);

            controller.When(sub => { String get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns("CurrentAccount");
        }

        #region Method: Edit()

        [Fact]
        public void Edit_OnGetRedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToActionResult actual = controller.Edit() as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Edit_ReturnsCurrentProfileEditView()
        {
            service.Get<ProfileEditView>(controller.CurrentAccountId).Returns(new ProfileEditView());
            service.IsActive(controller.CurrentAccountId).Returns(true);

            Object expected = service.Get<ProfileEditView>(controller.CurrentAccountId);
            Object actual = (controller.Edit() as ViewResult).ViewData.Model;

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
        public void Edit_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToActionResult actual = controller.Edit(null) as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(false);

            Object actual = (controller.Edit(profileEdit) as ViewResult).ViewData.Model;
            Object expected = profileEdit;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Edit_EditsProfileView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(true);

            controller.Edit(profileEdit);

            service.Received().Edit(profileEdit);
        }

        [Fact]
        public void Edit_AddsProfileUpdatedMessage()
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
        public void Edit_AfterEditRedirectsToEdit()
        {
            validator.CanEdit(profileEdit).Returns(true);
            service.IsActive(controller.CurrentAccountId).Returns(true);

            RedirectToActionResult actual = controller.Edit(profileEdit) as RedirectToActionResult;

            Assert.Equal("Edit", actual.ActionName);
            Assert.Null(actual.ControllerName);
            Assert.Empty(actual.RouteValues);
        }

        #endregion

        #region Method: Delete()

        [Fact]
        public void Delete_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToActionResult actual = controller.Delete() as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void Delete_AddsDeleteDisclaimerMessage()
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

            Object model = (controller.Delete() as ViewResult).ViewData.Model;

            Assert.Null(model);
        }

        #endregion

        #region Method: DeleteConfirmed(ProfileDeleteView profile)

        [Fact]
        public void DeleteConfirmed_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "DeleteConfirmed");
        }

        [Fact]
        public void DeleteConfirmed_RedirectsToLogoutIfAccountIsNotActive()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToActionResult actual = controller.DeleteConfirmed(profileDelete) as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        [Fact]
        public void DeleteConfirmed_AddsDeleteDisclaimerMessageIfCanNotDelete()
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
        public void DeleteConfirmed_IfCanNotDeleteReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(false);

            Object model = (controller.DeleteConfirmed(profileDelete) as ViewResult).ViewData.Model;

            Assert.Null(model);
        }

        [Fact]
        public void DeleteConfirmed_DeletesProfileView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(true);

            controller.DeleteConfirmed(profileDelete);

            service.Received().Delete(controller.CurrentAccountId);
        }

        [Fact]
        public void DeleteConfirmed_AfterDeleteRedirectsToAuthLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);
            validator.CanDelete(profileDelete).Returns(true);

            RedirectToActionResult actual = controller.DeleteConfirmed(profileDelete) as RedirectToActionResult;

            Assert.Equal("Auth", actual.ControllerName);
            Assert.Equal("Logout", actual.ActionName);
            Assert.Empty(actual.RouteValues);
        }

        #endregion
    }
}
