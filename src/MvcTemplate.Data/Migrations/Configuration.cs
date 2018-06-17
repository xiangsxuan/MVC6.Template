using Microsoft.EntityFrameworkCore;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;
using System.Linq;

namespace MvcTemplate.Data.Migrations
{
    public sealed class Configuration : IDisposable
    {
        private IUnitOfWork UnitOfWork { get; }
        private DbContext Context { get; }

        public Configuration(DbContext context, DbContext audit)
        {
            IAuditLogger logger = new AuditLogger(audit, 0);
            UnitOfWork = new UnitOfWork(context, logger);
            Context = context;
        }

        public void UpdateDatabase()
        {
            Context.Database.Migrate();

            SeedPermissions();
            SeedRoles();

            SeedAccounts();
        }

        #region Administration

        private void SeedPermissions()
        {
            Permission[] permissions =
            {
                new Permission { Id = 1, Area = "Administration", Controller = "Accounts", Action = "Index" },
                new Permission { Id = 2, Area = "Administration", Controller = "Accounts", Action = "Create" },
                new Permission { Id = 3, Area = "Administration", Controller = "Accounts", Action = "Details" },
                new Permission { Id = 4, Area = "Administration", Controller = "Accounts", Action = "Edit" },

                new Permission { Id = 5, Area = "Administration", Controller = "Roles", Action = "Index" },
                new Permission { Id = 6, Area = "Administration", Controller = "Roles", Action = "Create" },
                new Permission { Id = 7, Area = "Administration", Controller = "Roles", Action = "Details" },
                new Permission { Id = 8, Area = "Administration", Controller = "Roles", Action = "Edit" },
                new Permission { Id = 9, Area = "Administration", Controller = "Roles", Action = "Delete" }
            };

            Permission[] currentPermissions = UnitOfWork.Select<Permission>().ToArray();
            foreach (Permission permission in currentPermissions)
            {
                if (permissions.All(perm => perm.Id != permission.Id))
                {
                    UnitOfWork.DeleteRange(UnitOfWork.Select<RolePermission>().Where(role => role.PermissionId == permission.Id));
                    UnitOfWork.Delete(permission);
                }
            }

            foreach (Permission permission in permissions)
            {
                Permission currentPermission = currentPermissions.SingleOrDefault(perm => perm.Id == permission.Id);
                if (currentPermission != null)
                {
                    currentPermission.Controller = permission.Controller;
                    currentPermission.Action = permission.Action;
                    currentPermission.Area = permission.Area;

                    UnitOfWork.Update(currentPermission);
                }
                else
                {
                    UnitOfWork.Insert(permission);
                }
            }

            UnitOfWork.Commit();
        }

        private void SeedRoles()
        {
            if (!UnitOfWork.Select<Role>().Any(role => role.Title == "Sys_Admin"))
            {
                UnitOfWork.Insert(new Role { Title = "Sys_Admin" });
                UnitOfWork.Commit();
            }

            Int32 admin = UnitOfWork.Select<Role>().Single(role => role.Title == "Sys_Admin").Id;
            RolePermission[] currentPermissions = UnitOfWork
                .Select<RolePermission>()
                .Where(rolePermission => rolePermission.RoleId == admin)
                .ToArray();

            foreach (Permission permission in UnitOfWork.Select<Permission>())
                if (currentPermissions.All(rolePermission => rolePermission.PermissionId != permission.Id))
                    UnitOfWork.Insert(new RolePermission
                    {
                        RoleId = admin,
                        PermissionId = permission.Id
                    });

            UnitOfWork.Commit();
        }

        private void SeedAccounts()
        {
            Account[] accounts =
            {
                new Account
                {
                    Username = "admin",
                    Passhash = "$2b$13$ouxA6L7QZ/eSeVZD8lawSOEwtRn/hOoRY67Pwaj/WJaZe7S4.cHJC", // Will be generated on project rename
                    Email = "admin@test.domains.com",
                    IsLocked = false,

                    RoleId = UnitOfWork.Select<Role>().Single(role => role.Title == "Sys_Admin").Id
                }
            };

            foreach (Account account in accounts)
            {
                Account currentAccount = UnitOfWork.Select<Account>().FirstOrDefault(model => model.Username == account.Username);
                if (currentAccount != null)
                {
                    currentAccount.IsLocked = account.IsLocked;
                    currentAccount.RoleId = account.RoleId;

                    UnitOfWork.Update(currentAccount);
                }
                else
                {
                    UnitOfWork.Insert(account);
                }
            }

            UnitOfWork.Commit();
        }

        #endregion

        public void Dispose()
        {
            UnitOfWork.Dispose();
            Context.Dispose();
        }
    }
}
