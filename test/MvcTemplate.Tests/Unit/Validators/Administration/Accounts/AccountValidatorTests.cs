using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.Administration.Accounts.AccountView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Validators
{
    public class AccountValidatorTests : IDisposable
    {
        private AccountValidator validator;
        private TestingContext context;
        private Account account;
        private IHasher hasher;

        public AccountValidatorTests()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(true);

            TearDownData();
            SetUpData();

            validator = new AccountValidator(new UnitOfWork(context), hasher);
            validator.CurrentAccountId = account.Id;
        }
        public void Dispose()
        {
            validator.Dispose();
            context.Dispose();
        }

        #region Method: CanRegister(AccountRegisterView view)

        [Fact]
        public void CanRegister_CanNotRegisterWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanRegister(ObjectFactory.CreateAccountRegisterView()));
        }

        [Fact]
        public void CanRegister_CanNotRegisterWithAlreadyTakenUsername()
        {
            AccountRegisterView view = ObjectFactory.CreateAccountRegisterView(2);
            view.Username = account.Username.ToLower();

            Boolean canRegister = validator.CanRegister(view);

            Assert.False(canRegister);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.UsernameIsAlreadyTaken, validator.ModelState["Username"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanRegister_CanNotRegisterWithAlreadyUsedEmail()
        {
            AccountRegisterView view = ObjectFactory.CreateAccountRegisterView(2);
            view.Email = account.Email;

            Boolean canRegister = validator.CanRegister(view);

            Assert.False(canRegister);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.EmailIsAlreadyUsed, validator.ModelState["Email"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanRegister_CanRegisterValidAccount()
        {
            Assert.True(validator.CanRegister(ObjectFactory.CreateAccountRegisterView(2)));
        }

        #endregion

        #region Method: CanRecover(AccountRecoveryView view)

        [Fact]
        public void CanRecover_CanNotRecoverWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        [Fact]
        public void CanRecover_CanRecoverValidAccount()
        {
            Assert.True(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanReset(AccountResetView view)

        [Fact]
        public void CanReset_CanNotResetWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanReset(ObjectFactory.CreateAccountResetView()));
        }

        [Fact]
        public void CanReset_CanNotResetWithExpiredToken()
        {
            Account account = context.Set<Account>().Single();
            AccountResetView view = ObjectFactory.CreateAccountResetView();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(-5);
            context.SaveChanges();

            Boolean canReset = validator.CanReset(view);
            Alert alert = validator.Alerts.Single();

            Assert.False(canReset);
            Assert.Empty(validator.ModelState);
            Assert.Equal(0, alert.FadeoutAfter);
            Assert.Equal(AlertType.Danger, alert.Type);
            Assert.Equal(Validations.RecoveryTokenExpired, alert.Message);
        }

        [Fact]
        public void CanReset_CanResetValidAccount()
        {
            Assert.True(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanLogin(AccountLoginView view)

        [Fact]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        [Fact]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            hasher.VerifyPassword(null, null).Returns(false);
            AccountLoginView view = new AccountLoginView();
            view.Username = "Test";

            Boolean canLogin = validator.CanLogin(view);

            Assert.False(canLogin);
            Assert.Empty(validator.Alerts);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectAuthentication, validator.ModelState[""].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            Account account = context.Set<Account>().Single();
            account.IsLocked = true;
            context.SaveChanges();

            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            hasher.VerifyPassword(view.Password, Arg.Any<String>()).Returns(false);

            Boolean canLogin = validator.CanLogin(view);

            Assert.False(canLogin);
            Assert.Empty(validator.Alerts);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectAuthentication, validator.ModelState[""].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            view.Username = view.Username.ToUpper();

            Assert.True(validator.CanLogin(view));
        }

        [Fact]
        public void CanLogin_CanNotLoginWithLockedAccount()
        {
            AccountLoginView view = ObjectFactory.CreateAccountLoginView();
            Account account = context.Set<Account>().Single();
            account.IsLocked = true;
            context.SaveChanges();

            Boolean canLogin = validator.CanLogin(view);
            Alert alert = validator.Alerts.Single();

            Assert.False(canLogin);
            Assert.Empty(validator.ModelState);
            Assert.Equal(0, alert.FadeoutAfter);
            Assert.Equal(AlertType.Danger, alert.Type);
            Assert.Equal(Validations.AccountIsLocked, alert.Message);
        }

        [Fact]
        public void CanLogin_CanLoginWithValidAccount()
        {
            Assert.True(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        #endregion

        #region Method: CanCreate(AccountCreateView view)

        [Fact]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectFactory.CreateAccountCreateView()));
        }

        [Fact]
        public void CanCreate_CanNotCreateWithAlreadyTakenUsername()
        {
            AccountCreateView view = ObjectFactory.CreateAccountCreateView(2);
            view.Username = account.Username.ToLower();

            Boolean canCreate = validator.CanCreate(view);

            Assert.False(canCreate);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.UsernameIsAlreadyTaken, validator.ModelState["Username"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanCreate_CanNotCreateWithAlreadyUsedEmail()
        {
            AccountCreateView view = ObjectFactory.CreateAccountCreateView(2);
            view.Email = account.Email.ToUpper();

            Boolean canCreate = validator.CanCreate(view);

            Assert.False(canCreate);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.EmailIsAlreadyUsed, validator.ModelState["Email"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanCreate_CanCreateValidAccount()
        {
            Assert.True(validator.CanCreate(ObjectFactory.CreateAccountCreateView(2)));
        }

        #endregion

        #region Method: CanEdit(AccountEditView view)

        [Fact]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        [Fact]
        public void CanEdit_CanEditValidAccount()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        #endregion

        #region Method: CanEdit(ProfileEditView view)

        [Fact]
        public void CanEdit_CanNotEditProfileWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        [Fact]
        public void CanEdit_CanNotEditWithIncorrectPassword()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView(1577);
            hasher.VerifyPassword(view.Password, Arg.Any<String>()).Returns(false);

            Boolean canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectPassword, validator.ModelState["Password"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount(2);
            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            view.Username = takenAccount.Username.ToLower();

            Boolean canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.UsernameIsAlreadyTaken, validator.ModelState["Username"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_CanEditUsingItsOwnUsername()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView(2);
            view.Username = account.Username.ToUpper();

            Assert.True(validator.CanEdit(view));
        }

        [Fact]
        public void CanEdit_CanNotEditToAlreadyUsedEmail()
        {
            Account usedEmailAccount = ObjectFactory.CreateAccount(2);
            context.Set<Account>().Add(usedEmailAccount);
            context.SaveChanges();

            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            view.Email = usedEmailAccount.Email;

            Boolean canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.EmailIsAlreadyUsed, validator.ModelState["Email"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_CanEditUsingItsOwnEmail()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView(2);
            view.Email = account.Email.ToUpper();

            Assert.True(validator.CanEdit(view));
        }

        [Fact]
        public void CanEdit_CanEditValidProfile()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateProfileEditView(1457)));
        }

        #endregion

        #region Method: CanDelete(ProfileDeleteView view)

        [Fact]
        public void CanEdit_CanNotDeleteWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        [Fact]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            ProfileDeleteView view = ObjectFactory.CreateProfileDeleteView();
            hasher.VerifyPassword(view.Password, Arg.Any<String>()).Returns(false);

            Boolean canDelete = validator.CanDelete(view);

            Assert.False(canDelete);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validations.IncorrectPassword, validator.ModelState["Password"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanDelete_CanDeleteValidProfile()
        {
            Assert.True(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.Role = ObjectFactory.CreateRole();
            account.RoleId = account.Role.Id;
            account.IsLocked = false;

            context.Set<Role>().Add(account.Role);
            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}
