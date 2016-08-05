using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MvcTemplate.Components.Lookups;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Lookups
{
    public class LookupOfTests
    {
        private LookupOf<Role, RoleView> lookup;
        private IUrlHelper url;

        public LookupOfTests()
        {
            url = Substitute.For<IUrlHelper>();
            lookup = new LookupOf<Role, RoleView>(url);
            using (TestingContext context = new TestingContext()) context.DropData();
        }

        #region LookupOf(IUrlHelper url)

        [Fact]
        public void LookupOf_SetsTitle()
        {
            lookup = new LookupOf<Role, RoleView>(url);

            String expected = ResourceProvider.GetLookupTitle(typeof(RoleView).Name.Replace("View", ""));
            String actual = lookup.Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LookupOf_SetsUrl()
        {
            url.Action(Arg.Is<UrlActionContext>(context => context.Action == typeof(Role).Name && context.Controller == "Lookup")).Returns("Test");
            lookup = new LookupOf<Role, RoleView>(url);

            String actual = lookup.Url;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetColumnHeader(PropertyInfo property)

        [Fact]
        public void GetColumnHeader_ReturnsPropertyTitle()
        {
            String actual = lookup.GetColumnHeader(typeof(RoleView).GetProperty("Title"));
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsRelationPropertyTitle()
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty("Child");

            String actual = lookup.GetColumnHeader(property);
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetColumnCssClass(PropertyInfo property)

        [Theory]
        [InlineData("EnumField", "text-left")]
        [InlineData("SByteField", "text-right")]
        [InlineData("ByteField", "text-right")]
        [InlineData("Int16Field", "text-right")]
        [InlineData("UInt16Field", "text-right")]
        [InlineData("Int32Field", "text-right")]
        [InlineData("UInt32Field", "text-right")]
        [InlineData("Int64Field", "text-right")]
        [InlineData("UInt64Field", "text-right")]
        [InlineData("SingleField", "text-right")]
        [InlineData("DoubleField", "text-right")]
        [InlineData("DecimalField", "text-right")]
        [InlineData("DateTimeField", "text-center")]

        [InlineData("NullableEnumField", "text-left")]
        [InlineData("NullableSByteField", "text-right")]
        [InlineData("NullableByteField", "text-right")]
        [InlineData("NullableInt16Field", "text-right")]
        [InlineData("NullableUInt16Field", "text-right")]
        [InlineData("NullableInt32Field", "text-right")]
        [InlineData("NullableUInt32Field", "text-right")]
        [InlineData("NullableInt64Field", "text-right")]
        [InlineData("NullableUInt64Field", "text-right")]
        [InlineData("NullableSingleField", "text-right")]
        [InlineData("NullableDoubleField", "text-right")]
        [InlineData("NullableDecimalField", "text-right")]
        [InlineData("NullableDateTimeField", "text-center")]

        [InlineData("StringField", "text-left")]
        [InlineData("Child", "text-left")]
        public void GetColumnCssClass_ReturnsCssClassForPropertyType(String propertyName, String cssClass)
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

            String actual = lookup.GetColumnCssClass(property);
            String expected = cssClass;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetModels()

        [Fact]
        public void GetModels_FromUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            unitOfWork.Select<Role>().To<RoleView>().Returns(new RoleView[0].AsQueryable());

            Object actual = new LookupOf<Role, RoleView>(unitOfWork).GetModels();
            Object expected = unitOfWork.Select<Role>().To<RoleView>();

            Assert.Same(expected, actual);
        }

        #endregion

        #region FilterById(IQueryable<TView> models)

        [Fact]
        public void FilterById_NotInteger_ReturnsEmpty()
        {
            lookup.Filter.Id = "A";

            Assert.Empty(lookup.FilterById(null));
        }

        [Fact]
        public void FilterById_FromCurrentFilter()
        {
            TestingContext context = new TestingContext();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(context);
            lookup = new LookupOf<Role, RoleView>(unitOfWork);

            lookup.Filter.Id = role.Id.ToString();

            RoleView expected = unitOfWork.Select<Role>().To<RoleView>().Single();
            RoleView actual = lookup.FilterById(null).Single();

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion
    }
}
