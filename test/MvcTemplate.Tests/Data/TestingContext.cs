using Microsoft.EntityFrameworkCore;
using MvcTemplate.Data.Core;
using System;

namespace MvcTemplate.Tests
{
    public class TestingContext : Context
    {
        #region Tests

        protected DbSet<TestModel> TestModel { get; set; }

        #endregion

        public String DatabaseName { get; }

        public TestingContext()
            : this(Guid.NewGuid().ToString())
        {
        }
        public TestingContext(String databaseName)
        {
            DatabaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseInMemoryDatabase(DatabaseName);
        }
    }
}
