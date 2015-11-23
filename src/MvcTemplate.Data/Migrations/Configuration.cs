using Microsoft.Data.Entity;
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
        private Boolean Disposed { get; set; }

        public Configuration(DbContext context)
        {
            IAuditLogger logger = new AuditLogger(new Context(), "sys_seeder");
            UnitOfWork = new UnitOfWork(context, logger);
        }

        public void Seed()
        {
            SeedPrivileges();
            SeedRoles();

            SeedAccounts();
        }

        #region Administration

        private void SeedPrivileges()
        {
            Privilege[] privileges =
            {
                new Privilege { Id = "00000000-0000-0000-0000-000000000001", Area = "Administration", Controller = "Accounts", Action = "Index" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000002", Area = "Administration", Controller = "Accounts", Action = "Create" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000003", Area = "Administration", Controller = "Accounts", Action = "Details" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000004", Area = "Administration", Controller = "Accounts", Action = "Edit" },

                new Privilege { Id = "00000000-0000-0000-0000-000000000005", Area = "Administration", Controller = "Roles", Action = "Index" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000006", Area = "Administration", Controller = "Roles", Action = "Create" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000007", Area = "Administration", Controller = "Roles", Action = "Details" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000008", Area = "Administration", Controller = "Roles", Action = "Edit" },
                new Privilege { Id = "00000000-0000-0000-0000-000000000009", Area = "Administration", Controller = "Roles", Action = "Delete" }
            };

            Privilege[] currentPrivileges = UnitOfWork.Select<Privilege>().ToArray();
            foreach (Privilege privilege in currentPrivileges)
            {
                if (!privileges.Any(priv => priv.Id == privilege.Id))
                {
                    foreach (RolePrivilege rolePrivilege in UnitOfWork.Select<RolePrivilege>().Where(role => role.PrivilegeId == privilege.Id))
                        UnitOfWork.Delete(rolePrivilege);

                    UnitOfWork.Delete(privilege);
                }
            }

            foreach (Privilege privilege in privileges)
            {
                Privilege currentPrivilege = currentPrivileges.SingleOrDefault(priv => priv.Id == privilege.Id);
                if (currentPrivilege == null)
                {
                    UnitOfWork.Insert(privilege);
                }
                else
                {
                    currentPrivilege.Controller = privilege.Controller;
                    currentPrivilege.Action = privilege.Action;
                    currentPrivilege.Area = privilege.Area;

                    UnitOfWork.Update(currentPrivilege);
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

            String adminRoleId = UnitOfWork.Select<Role>().Single(role => role.Title == "Sys_Admin").Id;
            RolePrivilege[] adminPrivileges = UnitOfWork
                .Select<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == adminRoleId)
                .ToArray();

            foreach (Privilege privilege in UnitOfWork.Select<Privilege>())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    UnitOfWork.Insert(new RolePrivilege
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
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
                    Passhash = "$2a$13$yTgLCqGqgH.oHmfboFCjyuVUy5SJ2nlyckPFEZRJQrMTZWN.f1Afq", // Admin123?
                    Email = "admin@admins.com",
                    IsLocked = false,

                    RoleId = UnitOfWork.Select<Role>().Single(role => role.Title == "Sys_Admin").Id
                }
            };

            foreach (Account account in accounts)
            {
                Account dbAccount = UnitOfWork.Select<Account>().FirstOrDefault(acc => acc.Username == account.Username);
                if (dbAccount != null)
                {
                    dbAccount.IsLocked = account.IsLocked;

                    UnitOfWork.Update(dbAccount);
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();

            Disposed = true;
        }
    }
}
