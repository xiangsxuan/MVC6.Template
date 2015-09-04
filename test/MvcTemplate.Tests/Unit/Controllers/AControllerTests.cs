using Microsoft.AspNet.Mvc;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Controllers;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class AControllerTests
    {
        protected void ReturnsCurrentAccountId(BaseController controller, String id)
        {
            controller.When(sub => { String get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns(id);
        }

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

        protected RedirectToActionResult NotEmptyView(BaseController controller, Object model)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.NotEmptyView(model)).DoNotCallBase();
            controller.NotEmptyView(model).Returns(result);

            return result;
        }

        protected RedirectToActionResult RedirectToDefault(BaseController controller)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(result);

            return result;
        }
        protected RedirectToActionResult RedirectToNotFound(BaseController controller)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            controller.RedirectToNotFound().Returns(result);

            return result;
        }
        protected RedirectToActionResult RedirectIfAuthorized(BaseController controller, String action)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectIfAuthorized(action)).DoNotCallBase();
            controller.RedirectIfAuthorized(action).Returns(result);

            return result;
        }
    }
}
