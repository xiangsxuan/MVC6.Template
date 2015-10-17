using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Migrations
{
    public class InitialDataTests : IDisposable
    {
        private Context context;

        public InitialDataTests()
        {
            context = new Context();
        }
        public void Dispose()
        {
            context.Dispose();
        }

        #region Table: Roles

        [Fact]
        public void RolesTable_HasSysAdmin()
        {
            Assert.NotNull(context.Set<Role>().SingleOrDefault(role => role.Title == "Sys_Admin"));
        }

        #endregion

        #region Table: Accounts

        [Fact]
        public void AccountsTable_HasSysAdmin()
        {
            Assert.NotNull(context.Set<Account>()
                .SingleOrDefault(account =>
                    account.Username == "admin" &&
                    account.Role.Title == "Sys_Admin"));
        }

        #endregion

        #region Table: Privileges

        [Theory]
        [InlineData("Administration", "Accounts", "Index")]
        [InlineData("Administration", "Accounts", "Create")]
        [InlineData("Administration", "Accounts", "Details")]
        [InlineData("Administration", "Accounts", "Edit")]

        [InlineData("Administration", "Roles", "Index")]
        [InlineData("Administration", "Roles", "Create")]
        [InlineData("Administration", "Roles", "Details")]
        [InlineData("Administration", "Roles", "Edit")]
        [InlineData("Administration", "Roles", "Delete")]
        public void PrivilegesTable_HasPrivilege(String area, String controller, String action)
        {
            Assert.NotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Controller == controller &&
                privilege.Action == action &&
                privilege.Area == area));
        }

        [Fact]
        public void PrivilegesTable_HasExactNumberOfPrivileges()
        {
            Int32 actual = context.Set<Privilege>().Count();
            Int32 expected = 9;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Table: RolePrivileges

        [Fact]
        public void RolesPrivilegesTable_HasAllSysAdminPrivileges()
        {
            IEnumerable<String> expected = context
                .Set<Privilege>()
                .Select(privilege => privilege.Id)
                .OrderBy(privilegeId => privilegeId);

            IEnumerable<String> actual = context
                .Set<RolePrivilege>()
                .Where(privilege => privilege.Role.Title == "Sys_Admin")
                .Select(rolePrivilege => rolePrivilege.PrivilegeId)
                .OrderBy(privilegeId => privilegeId);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
