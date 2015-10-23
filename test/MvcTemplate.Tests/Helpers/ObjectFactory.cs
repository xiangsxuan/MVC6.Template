using MvcTemplate.Objects;
using MvcTemplate.Tests.Objects;
using System;

namespace MvcTemplate.Tests
{
    public static class ObjectFactory
    {
        #region Administration

        public static Account CreateAccount(Int32 id = 1)
        {
            return new Account
            {
                Id = "Id" + id,

                Username = "Username" + id,
                Passhash = "Passhash" + id,

                Email = id + "@tests.com",

                IsLocked = true,

                RecoveryToken = "Token" + id,
                RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(5),

                RoleId = "Id" + id,
                Role = CreateRole(id)
            };
        }
        public static AccountView CreateAccountView(Int32 id = 1)
        {
            return new AccountView
            {
                Id = "Id" + id,

                Username = "Username" + id,
                Email = id + "@tests.com",

                IsLocked = true,

                RoleTitle = "Title" + id
            };
        }
        public static AccountEditView CreateAccountEditView(Int32 id = 1)
        {
            return new AccountEditView
            {
                Id = "Id" + id,

                Username = "Username" + id,
                Email = id + "@tests.com",

                IsLocked = true,

                RoleId = "Id" + id
            };
        }
        public static AccountCreateView CreateAccountCreateView(Int32 id = 1)
        {
            return new AccountCreateView
            {
                Id = "Id" + id,

                Username = "Username" + id,
                Password = "Password" + id,

                Email = id + "@tests.com",

                RoleId = "Id" + id
            };
        }

        public static AccountLoginView CreateAccountLoginView(Int32 id = 1)
        {
            return new AccountLoginView
            {
                Id = "Id" + id,

                Username = "Username" + id,
                Password = "Password" + id,
            };
        }
        public static AccountResetView CreateAccountResetView(Int32 id = 1)
        {
            return new AccountResetView
            {
                Id = "Id" + id,

                Token = "Token" + id,
                NewPassword = "NewPassword" + id,
            };
        }
        public static AccountRegisterView CreateAccountRegisterView(Int32 id = 1)
        {
            return new AccountRegisterView
            {
                Id = "Id" + id,

                Username = "Username" + id,
                Password = "Password" + id,

                Email = id + "@tests.com"
            };
        }
        public static AccountRecoveryView CreateAccountRecoveryView(Int32 id = 1)
        {
            return new AccountRecoveryView
            {
                Id = "Id" + id,

                Email = id + "@tests.com"
            };
        }

        public static ProfileEditView CreateProfileEditView(Int32 id = 1)
        {
            return new ProfileEditView
            {
                Id = "Id" + id,

                Email = id + "@tests.com",
                Username = "Username" + id,

                Password = "Password" + id,
                NewPassword = "NewPassword" + id,

            };
        }
        public static ProfileDeleteView CreateProfileDeleteView(Int32 id = 1)
        {
            return new ProfileDeleteView
            {
                Id = "Id" + id,

                Password = "Password" + id
            };
        }

        public static Role CreateRole(Int32 id = 1)
        {
            return new Role
            {
                Id = "Id" + id,

                Title = "Title" + id
            };
        }
        public static RoleView CreateRoleView(Int32 id = 1)
        {
            return new RoleView
            {
                Id = "Id" + id,

                Title = "Title" + id
            };
        }

        public static Privilege CreatePrivilege(Int32 id = 1)
        {
            return new Privilege
            {
                Id = "Id" + id,

                Area = "Area" + id,
                Action = "Action" + id,
                Controller = "Controller" + id
            };
        }
        public static RolePrivilege CreateRolePrivilege(Int32 id = 1)
        {
            return new RolePrivilege
            {
                Id = "Id" + id,

                RoleId = "Id" + id,
                Role = CreateRole(id),

                PrivilegeId = "Id" + id,
                Privilege = CreatePrivilege(id)
            };
        }

        #endregion

        #region Tests

        public static TestModel CreateTestModel(Int32 id = 1)
        {
            return new TestModel
            {
                Id = "Id" + id,

                Text = "Text" + id
            };
        }

        #endregion
    }
}
