using Microsoft.Data.Entity;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Mvc6TemplateTest;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
    }
}
