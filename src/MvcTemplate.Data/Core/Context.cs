using Microsoft.Data.Entity;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;

namespace MvcTemplate.Data.Core
{
    public class Context : DbContext
    {
        #region Administration

        protected DbSet<Role> Roles { get; set; }
        protected DbSet<Account> Accounts { get; set; }
        protected DbSet<Privilege> Privileges { get; set; }
        protected DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        #region System

        protected DbSet<Log> Logs { get; set; }
        protected DbSet<AuditLog> AuditLogs { get; set; }

        #endregion

        static Context()
        {
            ObjectMapper.MapObjects();
        }

        protected override void OnConfiguring(EntityOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Mvc6Template;Trusted_Connection=True;");
        }
    }
}
