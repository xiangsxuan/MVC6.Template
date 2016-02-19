using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using System.Linq;

namespace MvcTemplate.Data.Core
{
    public class Context : DbContext
    {
        #region Administration

        protected DbSet<Role> Roles { get; set; }
        protected DbSet<Account> Accounts { get; set; }
        protected DbSet<Permission> Permissions { get; set; }
        protected DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region System

        protected DbSet<Log> Logs { get; set; }

        #endregion

        static Context()
        {
            ObjectMapper.MapObjects();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableForeignKey key in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                key.DeleteBehavior = DeleteBehavior.Restrict;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Mvc6Template;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
    }
}
