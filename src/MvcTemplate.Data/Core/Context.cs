using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Data.Core
{
    public class Context : DbContext
    {
        #region Administration

        protected DbSet<Role> Role { get; set; }
        protected DbSet<Account> Account { get; set; }
        protected DbSet<Permission> Permission { get; set; }
        protected DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        protected IConfiguration Config { get; }

        static Context()
        {
            ObjectMapper.MapObjects();
        }
        public Context(IConfiguration config)
        {
            Config = config;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (IEntityType entity in builder.Model.GetEntityTypes())
                foreach (PropertyInfo property in entity.ClrType.GetProperties())
                {
                    IndexAttribute index = property.GetCustomAttribute<IndexAttribute>(false);
                    if (index != null) builder.Entity(entity.ClrType).HasIndex(property.Name).IsUnique(index.IsUnique);
                }

            builder.Entity<Permission>().Property(model => model.Id).ValueGeneratedNever();
            foreach (IMutableForeignKey key in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                key.DeleteBehavior = DeleteBehavior.Restrict;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Config["Data:Connection"]);
        }
    }
}
