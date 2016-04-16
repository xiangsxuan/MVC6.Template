using Microsoft.AspNet.Mvc;
using MvcTemplate.Controllers;
using NSubstitute;
using System;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public abstract class ControllerTests
    {
        protected void ReturnCurrentAccountId(BaseController controller, Int32 id)
        {
            controller.When(sub => { Int32 get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns(id);
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
