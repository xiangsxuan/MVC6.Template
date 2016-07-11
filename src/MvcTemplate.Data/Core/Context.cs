using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using System.Linq;

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
