using Microsoft.AspNetCore.Mvc;
using MvcTemplate.Components.Lookups;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using NonFactors.Mvc.Lookup;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class LookupControllerTests
    {
        private LookupController controller;
        private IUnitOfWork unitOfWork;
        private LookupFilter filter;
        private MvcLookup lookup;

        public LookupControllerTests()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            controller = Substitute.ForPartsOf<LookupController>(unitOfWork);

            lookup = Substitute.For<MvcLookup>();
            filter = new LookupFilter();
        }

        #region GetData(MvcLookup lookup, LookupFilter filter)

        [Fact]
        public void GetData_SetsFilter()
        {
            controller.GetData(lookup, filter);

            LookupFilter actual = lookup.Filter;
            LookupFilter expected = filter;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetData_ReturnsJsonResult()
        {
            lookup.GetData().Returns(new LookupData());

            Object actual = controller.GetData(lookup, filter).Value;
            Object expected = lookup.GetData();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Role(LookupFilter filter)

        [Fact]
        public void Role_ReturnsRolesData()
        {
            Object expected = GetData<MvcLookup<Role, RoleView>>(controller);
            Object actual = controller.Role(filter);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Dispose()

        [Fact]
        public void Dispose_UnitOfWork()
        {
            controller.Dispose();

            unitOfWork.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion

        #region Test helpers

        private JsonResult GetData<TLookup>(LookupController lookupController) where TLookup : MvcLookup
        {
            lookupController.When(sub => sub.GetData(Arg.Any<TLookup>(), filter)).DoNotCallBase();
            lookupController.GetData(Arg.Any<TLookup>(), filter).Returns(new JsonResult("Test"));

            return lookupController.GetData(null, filter);
        }

        #endregion
    }
}
