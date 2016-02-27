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
    public abstract class ControllerTests
    {
        protected void ReturnCurrentAccountId(BaseController controller, Int32 id)
        {
            controller.When(sub => { Int32 get = sub.CurrentAccountId; }).DoNotCallBase();
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

            Assert.NotNull(methodInfo.GetParameters()[0].IsDefined(typeof(BindExcludeIdAttribute), false));
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
        protected RedirectToActionResult RedirectToAction(BaseController controller, String actionName)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectToAction(actionName)).DoNotCallBase();
            controller.RedirectToAction(actionName).Returns(result);

            return result;
        }
        protected RedirectToActionResult RedirectToAction(BaseController controller, String actionName, String controllerName)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectToAction(actionName, controllerName)).DoNotCallBase();
            controller.RedirectToAction(actionName, controllerName).Returns(result);

            return result;
        }
    }
}
