using AutoMapper.QueryableExtensions;
using Microsoft.Data.Entity;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    public class SelectTests : IDisposable
    {
        private TestingContext context;
        private Select<Role> select;

        public SelectTests()
        {
            context = new TestingContext();
            select = new Select<Role>(context.Set<Role>());

            context.Set<Role>().Add(ObjectFactory.CreateRole());
            context.DropData();
        }
        public void Dispose()
        {
            context.Dispose();
        }

        #region Property: ElementType

        [Fact]
        public void ElementType_IsModelType()
        {
            Object actual = (select as IQueryable).ElementType;
            Object expected = typeof(Role);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Property: Expression

        [Fact]
        public void Expression_IsSetsExpression()
        {
            DbSet<Role> set = Substitute.For<DbSet<Role>, IQueryable>();
            TestingContext context = Substitute.For<TestingContext>();
            ((IQueryable)set).Expression.Returns(Expression.Empty());
            context.Set<Role>().Returns(set);

            select = new Select<Role>(context.Set<Role>());

            Object actual = ((IQueryable)select).Expression;
            Object expected = ((IQueryable)set).Expression;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Property: Provider

        [Fact]
        public void Provider_IsSetsProvider()
        {
            Object expected = (context.Set<Role>() as IQueryable).Provider;
            Object actual = (select as IQueryable).Provider;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Where(Expression<Func<TModel, Boolean>> predicate)

        [Fact]
        public void Where_Filters()
        {
            IEnumerable<Role> actual = select.Where(model => true);
            IEnumerable<Role> expected = context.Set<Role>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Where_ReturnsItself()
        {
            Object actual = select.Where(model => true);
            Object expected = select;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: To<TView>()

        [Fact]
        public void To_ProjectsSet()
        {
            IEnumerable<String> expected = context.Set<Role>().ProjectTo<RoleView>().Select(view => view.Id).ToArray();
            IEnumerable<String> actual = select.To<RoleView>().Select(view => view.Id).ToArray();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetEnumerator()

        [Fact]
        public void GetEnumerator_ReturnsSetEnumerator()
        {
            IEnumerable<Role> expected = context.Set<Role>();
            IEnumerable<Role> actual = select.ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEnumerator_ReturnsSameEnumerator()
        {
            IEnumerable<Role> expected = context.Set<Role>();
            IEnumerable<Role> actual = select;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
