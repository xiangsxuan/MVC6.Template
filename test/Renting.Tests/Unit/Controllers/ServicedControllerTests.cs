﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Renting.Components.Mvc;
using Renting.Services;
using NSubstitute;
using System;
using Xunit;

namespace Renting.Controllers.Tests
{
    public class ServicedControllerTests : ControllerTests
    {
        private ServicedController<IService> controller;
        private IService service;

        public ServicedControllerTests()
        {
            service = Substitute.For<IService>();
            controller = Substitute.ForPartsOf<ServicedController<IService>>(service);

            controller.ControllerContext.RouteData = new RouteData();
            controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
            controller.HttpContext.RequestServices.GetService(typeof(ILanguages)).Returns(Substitute.For<ILanguages>());
        }

        #region ServicedController(TService service)

        [Fact]
        public void ServicedController_SetsService()
        {
            Object actual = controller.Service;
            Object expected = service;

            Assert.Same(expected, actual);
        }

        #endregion

        #region OnActionExecuting(ActionExecutingContext context)

        [Fact]
        public void OnActionExecuting_SetsServiceCurrentAccountId()
        {
            ReturnCurrentAccountId(controller, 1);

            controller.OnActionExecuting(null);

            Int32 expected = controller.CurrentAccountId;
            Int32 actual = service.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Dispose()

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
