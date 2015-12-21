using Microsoft.Data.Entity;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data.Mapping;
using MvcTemplate.Tests.Objects;

namespace MvcTemplate.Tests.Data
{
    public class TestingContext : Context
    {
        #region Test

        protected DbSet<TestModel> TestModels { get; set; }

        #endregion

        static TestingContext()
        {
            TestObjectMapper.MapObjects();
        }

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
