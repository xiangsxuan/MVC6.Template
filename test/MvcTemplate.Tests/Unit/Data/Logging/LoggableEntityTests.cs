using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    public class LoggableEntityTests : IDisposable
    {
        private EntityEntry<BaseModel> entry;
        private TestingContext context;
        private TestModel model;

        public LoggableEntityTests()
        {
            using (context = new TestingContext())
            {
                context.RemoveRange(context.Set<TestModel>());
                context.DropData();

                model = ObjectFactory.CreateTestModel();
                context.Set<TestModel>().Add(model);
                context.SaveChanges();
            }

            context = new TestingContext();
            entry = context.Entry<BaseModel>(model);
        }
        public void Dispose()
        {
            context.Dispose();
        }

        #region LoggableEntity(EntityEntry<BaseModel> entry)

        [Fact]
        public void LoggableEntity_CreatesPropertiesForAddedEntity()
        {
            entry.State = EntityState.Added;

            AsssertProperties(entry.CurrentValues);
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForModifiedEntity()
        {
            String title = model.Title;
            entry.State = EntityState.Modified;
            entry.CurrentValues["Title"] = "Role";
            entry.OriginalValues["Title"] = "Role";

            IEnumerator<LoggableProperty> expected = new List<LoggableProperty> { new LoggableProperty(entry.Property("Title"), title) }.GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.IsModified, actual.Current.IsModified);
                Assert.Equal(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForAttachedEntity()
        {
            context.Dispose();
            String title = model.Title;
            context = new TestingContext();
            context.Set<TestModel>().Attach(model);

            entry = context.Entry<BaseModel>(model);
            entry.OriginalValues["Title"] = "Role";
            entry.CurrentValues["Title"] = "Role";
            entry.State = EntityState.Modified;

            IEnumerator<LoggableProperty> expected = new List<LoggableProperty> { new LoggableProperty(entry.Property("Title"), title) }.GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.IsModified, actual.Current.IsModified);
                Assert.Equal(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForDeletedEntity()
        {
            entry.State = EntityState.Deleted;

            AsssertProperties(entry.GetDatabaseValues());
        }

        [Fact]
        public void LoggableEntity_SetsAction()
        {
            entry.State = EntityState.Deleted;

            String actual = new LoggableEntity(entry).Action;
            String expected = entry.State.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityTypeName()
        {
            String actual = new LoggableEntity(entry).Name;
            String expected = typeof(TestModel).Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityId()
        {
            Int32 actual = new LoggableEntity(entry).Id();
            Int32 expected = model.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region ToString()

        [Fact]
        public void ToString_FormsEntityChanges()
        {
            StringBuilder changes = new StringBuilder();
            LoggableEntity loggableEntity = new LoggableEntity(entry);
            foreach (LoggableProperty property in loggableEntity.Properties)
                changes.AppendFormat("{0}{1}", property, Environment.NewLine);

            String actual = loggableEntity.ToString();
            String expected = changes.ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Test helpers

        private void AsssertProperties(PropertyValues newValues)
        {
            IEnumerable<IProperty> properties = newValues.Properties;

            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();
            IEnumerator<LoggableProperty> expected = properties.Where(property => property.Name != "Id")
                .Select(property => new LoggableProperty(entry.Property(property.Name), newValues[property])).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.IsModified, actual.Current.IsModified);
                Assert.Equal(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        #endregion
    }
}
