using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    public class UnitOfWorkTests : IDisposable
    {
        private TestingContext context;
        private UnitOfWork unitOfWork;
        private Role model;

        public UnitOfWorkTests()
        {
            context = new TestingContext();
            model = ObjectFactory.CreateRole();
            unitOfWork = new UnitOfWork(context);

            context.DropData();
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
            context.Dispose();
        }

        #region GetAs<TModel, TDestination>(Int32 id)

        [Fact]
        public void GetAs_ReturnsModelAsDestinationModelById()
        {
            context.Add(model);
            context.SaveChanges();

            RoleView expected = Mapper.Map<RoleView>(model);
            RoleView actual = unitOfWork.GetAs<Role, RoleView>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Get<TModel>(Int32 id)

        [Fact]
        public void Get_ModelById()
        {
            context.Add(model);
            context.SaveChanges();

            Role expected = context.Set<Role>().AsNoTracking().Single();
            Role actual = unitOfWork.Get<Role>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Get_NotFound_ReturnsNull()
        {
            Assert.Null(unitOfWork.Get<Role>(0));
        }

        #endregion

        #region To<TDestination>(Object source)

        [Fact]
        public void To_ConvertsSourceToDestination()
        {
            RoleView actual = unitOfWork.To<RoleView>(model);
            RoleView expected = Mapper.Map<RoleView>(model);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Select<TModel>()

        [Fact]
        public void Select_FromSet()
        {
            context.Add(model);
            context.SaveChanges();

            IEnumerable<Role> actual = unitOfWork.Select<Role>();
            IEnumerable<Role> expected = context.Set<Role>();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region InsertRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void InsertRange_AddsModelsToDbSet()
        {
            IEnumerable<Role> models = new[] { ObjectFactory.CreateRole(1), ObjectFactory.CreateRole(2) };
            unitOfWork.InsertRange(models);

            IEnumerator<Role> actual = context.ChangeTracker.Entries<Role>().Select(entry => entry.Entity).GetEnumerator();
            IEnumerator<Role> expected = models.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(EntityState.Added, context.Entry(actual.Current).State);
                Assert.Same(expected.Current, actual.Current);
            }
        }

        #endregion

        #region Insert<TModel>(TModel model)

        [Fact]
        public void Insert_AddsModelToDbSet()
        {
            unitOfWork.Insert(model);

            Object actual = context.ChangeTracker.Entries<Role>().Single().Entity;
            Object expected = model;

            Assert.Equal(EntityState.Added, context.Entry(model).State);
            Assert.Same(expected, actual);
        }

        #endregion

        #region Update<TModel>(TModel model)

        [Theory]
        [InlineData(EntityState.Added, EntityState.Modified)]
        [InlineData(EntityState.Deleted, EntityState.Modified)]
        [InlineData(EntityState.Detached, EntityState.Modified)]
        [InlineData(EntityState.Modified, EntityState.Modified)]
        [InlineData(EntityState.Unchanged, EntityState.Unchanged)]
        public void Update_Entry(EntityState initialState, EntityState state)
        {
            EntityEntry<Role> entry = context.Entry(model);
            entry.State = initialState;
            entry.Entity.Id = 0;

            unitOfWork.Update(model);

            EntityEntry<Role> actual = entry;

            Assert.Equal(state, actual.State);
            Assert.False(actual.Property(prop => prop.CreationDate).IsModified);
        }

        #endregion

        #region DeleteRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void DeleteRange_Models()
        {
            IEnumerable<Role> models = new[] { ObjectFactory.CreateRole(1), ObjectFactory.CreateRole(2) };

            context.AddRange(models);
            context.SaveChanges();

            unitOfWork.DeleteRange(models);
            unitOfWork.Commit();

            Assert.Empty(context.Set<Role>());
        }

        #endregion

        #region Delete<TModel>(TModel model)

        [Fact]
        public void Delete_Model()
        {
            context.Add(model);
            context.SaveChanges();

            unitOfWork.Delete(model);
            unitOfWork.Commit();

            Assert.Empty(context.Set<Role>());
        }

        #endregion

        #region Delete<TModel>(Int32 id)

        [Fact]
        public void Delete_ModelById()
        {
            context.Add(model);
            context.SaveChanges();

            unitOfWork.Delete<Role>(model.Id);
            unitOfWork.Commit();

            Assert.Empty(context.Set<Role>());
        }

        #endregion

        #region Commit()

        [Fact]
        public void Commit_SavesChanges()
        {
            TestingContext testingContext = Substitute.For<TestingContext>();

            new UnitOfWork(testingContext).Commit();

            testingContext.Received().SaveChanges();
        }

        #endregion

        #region Dispose()

        [Fact]
        public void Dispose_Context()
        {
            TestingContext testingContext = Substitute.For<TestingContext>();

            new UnitOfWork(testingContext).Dispose();

            testingContext.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion
    }
}
