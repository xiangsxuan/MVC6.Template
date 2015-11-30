using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Routing;
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
    public class BaseLookupTests
    {
        private BaseLookupProxy<Role, RoleView> lookup;
        private IUrlHelper urlHelper;

        public BaseLookupTests()
        {
            urlHelper = Substitute.For<IUrlHelper>();
            lookup = new BaseLookupProxy<Role, RoleView>(urlHelper);
            using (TestingContext context = new TestingContext()) context.DropData();
        }

        #region Constructor: BaseLookup(IUrlHelper url)

        [Fact]
        public void BaseLookup_SetsDialogTitle()
        {
            lookup = new BaseLookupProxy<Role, RoleView>(urlHelper);

            String expected = ResourceProvider.GetLookupTitle(typeof(RoleView).Name.Replace("View", ""));
            String actual = lookup.DialogTitle;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseLookup_SetsLookupUrl()
        {
            urlHelper.Action(Arg.Is<UrlActionContext>(context => context.Action == typeof(Role).Name && context.Controller == "Lookup")).Returns("Test");
            lookup = new BaseLookupProxy<Role, RoleView>(urlHelper);

            String actual = lookup.LookupUrl;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: BaseLookup(IUnitOfWork unitOfWork)

        [Fact]
        public void BaseLookup_SetsUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            lookup = new BaseLookupProxy<Role, RoleView>(unitOfWork);

            Object actual = lookup.BaseUnitOfWork;
            Object expected = unitOfWork;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Fact]
        public void GetColumnHeader_ReturnsPropertyTitle()
        {
            String actual = lookup.BaseGetColumnHeader(typeof(RoleView).GetProperty("Title"));
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsRelationPropertyTitle()
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty("Child");

            String actual = lookup.BaseGetColumnHeader(property);
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

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

            String actual = lookup.BaseGetColumnCssClass(property);
            String expected = cssClass;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetModels()

        [Fact]
        public void GetModels_FromUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            lookup = new BaseLookupProxy<Role, RoleView>(unitOfWork);
            unitOfWork.Select<Role>().To<RoleView>().Returns(new RoleView[0].AsQueryable());

            Object expected = unitOfWork.Select<Role>().To<RoleView>();
            Object actual = lookup.BaseGetModels();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: FilterById(IQueryable<TView> models)

        [Fact]
        public void FilterById_FromCurrentFilter()
        {
            TestingContext context = new TestingContext();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            IUnitOfWork unitOfWork = new UnitOfWork(context);
            lookup = new BaseLookupProxy<Role, RoleView>(unitOfWork);

            lookup.CurrentFilter.Id = role.Id;

            RoleView expected = unitOfWork.Select<Role>().To<RoleView>().Single();
            RoleView actual = lookup.BaseFilterById(null).Single();

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion
    }
}
