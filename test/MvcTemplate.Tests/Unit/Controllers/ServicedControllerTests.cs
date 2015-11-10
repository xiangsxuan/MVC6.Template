using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ServicedControllerTests : ControllerTests
    {
        private ServicedController<IService> controller;
        private IService service;

        public ServicedControllerTests()
        {
            service = Substitute.For<IService>();
            controller = Substitute.ForPartsOf<ServicedController<IService>>(service);
            controller.ActionContext.HttpContext = Substitute.For<HttpContext>();
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
            ReturnCurrentAccountId(controller, "Test");

            controller.OnActionExecuting(new ActionExecutingContext(new ActionContext(), null, null, null));

            String expected = controller.CurrentAccountId;
            String actual = service.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_Service()
        {
            controller.Dispose();

            service.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
