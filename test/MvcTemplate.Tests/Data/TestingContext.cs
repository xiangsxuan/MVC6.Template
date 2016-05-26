using Microsoft.EntityFrameworkCore;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;

namespace MvcTemplate.Tests.Data
{
    public class TestingContext : Context
    {
        public void DropData()
        {
            RemoveRange(Set<RolePermission>());
            RemoveRange(Set<Permission>());
            RemoveRange(Set<Account>());
            RemoveRange(Set<Role>());

            SaveChanges();
        }

        public TestingContext() : base(ConfigurationFactory.Create())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config["Data:TestConnection"]);
        }
    }
}
