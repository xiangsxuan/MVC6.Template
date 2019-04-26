using Microsoft.AspNetCore.Mvc;
using Renting.Components.Mvc;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Renting.Controllers.Tests
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

            Assert.True(methodInfo.GetParameters()[0].IsDefined(typeof(BindExcludeIdAttribute), false));
        }

        protected ViewResult NotFoundView(BaseController controller)
        {
            controller.When(sub => sub.NotFoundView()).DoNotCallBase();
            controller.NotFoundView().Returns(new ViewResult());

            return controller.NotFoundView();
        }
        protected ViewResult NotEmptyView(BaseController controller, Object model)
        {
            controller.When(sub => sub.NotEmptyView(model)).DoNotCallBase();
            controller.NotEmptyView(model).Returns(new ViewResult());

            return controller.NotEmptyView(model);
        }

        protected RedirectToActionResult RedirectToDefault(BaseController controller)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(result);

            return result;
        }
        protected RedirectToActionResult RedirectToAction(BaseController controller, String action)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            controller.When(sub => sub.RedirectToAction(action)).DoNotCallBase();
            controller.RedirectToAction(action).Returns(result);

            return result;
        }
        protected RedirectToActionResult RedirectToAction(BaseController baseController, String action, String controller)
        {
            RedirectToActionResult result = new RedirectToActionResult(null, null, null);
            baseController.When(sub => sub.RedirectToAction(action, controller)).DoNotCallBase();
            baseController.RedirectToAction(action, controller).Returns(result);

            return result;
        }
    }
}
