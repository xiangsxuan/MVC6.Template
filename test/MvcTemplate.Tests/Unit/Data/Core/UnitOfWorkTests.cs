using AutoMapper;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
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

        public UnitOfWorkTests()
        {
            context = new TestingContext();
            unitOfWork = new UnitOfWork(context);

            context.RemoveRange(context.Set<TestModel>());
            context.SaveChanges();
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
            context.Dispose();
        }

        #region Method: GetAs<TModel, TDestination>(String id)

        [Fact]
        public void GetAs_ReturnsModelAsDestinationModelById()
        {
            TestModel testModel = ObjectFactory.CreateTestModel();
            context.Add(testModel);
            context.SaveChanges();

            TestView expected = Mapper.Map<TestView>(context.Set<TestModel>().AsNoTracking().Single());
            TestView actual = unitOfWork.GetAs<TestModel, TestView>(testModel.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Text, actual.Text);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Get<TModel>(String id)

        [Fact]
        public void Get_ModelById()
        {
            TestModel testModel = ObjectFactory.CreateTestModel();
            context.Add(testModel);
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().AsNoTracking().Single();
            TestModel actual = unitOfWork.Get<TestModel>(testModel.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Text, actual.Text);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Get_NotFound_ReturnsNull()
        {
            Assert.Null(unitOfWork.Get<TestModel>(""));
        }

        #endregion

        #region Method: To<TDestination>(Object source)

        [Fact]
        public void To_ConvertsSourceToDestination()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            TestView actual = unitOfWork.To<TestView>(model);
            TestView expected = Mapper.Map<TestView>(model);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Text, actual.Text);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Select<TModel>()

        [Fact]
        public void Select_FromSet()
        {
            TestModel testModel = ObjectFactory.CreateTestModel();
            context.Add(testModel);
            context.SaveChanges();

            IEnumerable<TestModel> actual = unitOfWork.Select<TestModel>();
            IEnumerable<TestModel> expected = context.Set<TestModel>();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: InsertRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void InsertRange_AddsModelsToDbSet()
        {
            IEnumerable<TestModel> models = new[] { ObjectFactory.CreateTestModel(1), ObjectFactory.CreateTestModel(2) };
            unitOfWork.InsertRange(models);

            IEnumerator<TestModel> actual = context.ChangeTracker.Entries<TestModel>().Select(entry => entry.Entity).GetEnumerator();
            IEnumerator<TestModel> expected = models.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(EntityState.Added, context.Entry(actual.Current).State);
                Assert.Same(expected.Current, actual.Current);
            }
        }

        #endregion

        #region Method: Insert<TModel>(TModel model)

        [Fact]
        public void Insert_AddsModelToDbSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            unitOfWork.Insert(model);

            Object actual = context.ChangeTracker.Entries<TestModel>().Single().Entity;
            Object expected = model;

            Assert.Equal(EntityState.Added, context.Entry(model).State);
            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Fact]
        public void Update_UpdatesNotAttachedModel()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            model.Text += "Test";

            unitOfWork.Update(model);

            EntityEntry<TestModel> actual = context.Entry(model);
            TestModel expected = model;

            Assert.Equal(expected.CreationDate, actual.Entity.CreationDate);
            Assert.Equal(EntityState.Modified, actual.State);
            Assert.Equal(expected.Text, actual.Entity.Text);
            Assert.Equal(expected.Id, actual.Entity.Id);
        }

        [Fact]
        public void Update_DoesNotModifyCreationDate()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            unitOfWork.Update(model);

            Assert.False(context.Entry(model).Property(prop => prop.CreationDate).IsModified);
        }

        #endregion

        #region Method: DeleteRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void DeleteRange_Models()
        {
            IEnumerable<TestModel> models = new[] { ObjectFactory.CreateTestModel(1), ObjectFactory.CreateTestModel(2) };
            foreach (TestModel model in models)
                context.Add(model);

            context.SaveChanges();

            unitOfWork.DeleteRange(models);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Delete<TModel>(TModel model)

        [Fact]
        public void Delete_Model()
        {
            TestModel testModel = ObjectFactory.CreateTestModel();
            context.Add(testModel);
            context.SaveChanges();

            unitOfWork.Delete(testModel);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Delete<TModel>(String id)

        [Fact]
        public void Delete_ModelById()
        {
            TestModel testModel = ObjectFactory.CreateTestModel();
            context.Add(testModel);
            context.SaveChanges();

            unitOfWork.Delete<TestModel>(testModel.Id);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Rollback()

        [Fact]
        public void Rollback_Changes()
        {
            context.Add(ObjectFactory.CreateTestModel());

            unitOfWork.Rollback();
            unitOfWork.Commit();

            Assert.Empty(unitOfWork.Select<TestModel>());
        }

        #endregion

        #region Method: Commit()

        [Fact]
        public void Commit_SavesChanges()
        {
            TestingContext context = Substitute.For<TestingContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Commit();

            context.Received().SaveChanges();
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_Context()
        {
            TestingContext context = Substitute.For<TestingContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Dispose();

            context.Received().Dispose();
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
