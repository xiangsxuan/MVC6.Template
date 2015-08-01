using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Mvc;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class AControllerTests
    {
        protected void ProtectsFromOverpostingId(Controller controller, String postMethod)
        {
            MethodInfo methodInfo = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == postMethod &&
                    method.IsDefined(typeof(HttpPostAttribute), false));

            BindExcludeIdAttribute actual = methodInfo
                .GetParameters()
                .First()
                .GetCustomAttribute<BindExcludeIdAttribute>(false);

            Assert.NotNull(actual);
        }
    }
}
