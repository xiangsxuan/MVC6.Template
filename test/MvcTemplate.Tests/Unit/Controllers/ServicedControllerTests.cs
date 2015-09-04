using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ServicedControllerTests : AControllerTests
    {
        private ServicedController<IService> controller;
        private IService service;

        public ServicedControllerTests()
        {
            service = Substitute.For<IService>();
            controller = Substitute.ForPartsOf<ServicedController<IService>>(service);
        }

        #region Constructor: ServicedController(TService service)

        [Fact]
        public void ServicedController_SetsService()
        {
            Object actual = controller.Service;
            Object expected = service;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: OnActionExecuting(ActionExecutingContext context)

        [Fact]
        public void OnActionExecuting_SetsServiceCurrentAccountId()
        {
            ReturnsCurrentAccountId(controller, "Test");

            controller.OnActionExecuting(null);

            String expected = controller.CurrentAccountId;
            String actual = service.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesService()
        {
            controller.Dispose();

            service.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
