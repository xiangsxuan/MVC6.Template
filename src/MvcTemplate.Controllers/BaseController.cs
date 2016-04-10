using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        public IAuthorizationProvider AuthorizationProvider { get; protected set; }
        public virtual Int32 CurrentAccountId { get; protected set; }
        public AlertsContainer Alerts { get; protected set; }

        protected BaseController()
        {
            Alerts = new AlertsContainer();
        }

        public virtual ActionResult NotEmptyView(Object model)
        {
            if (model == null)
                return RedirectToNotFound();

            return View(model);
        }
        public virtual ActionResult RedirectToLocal(String url)
        {
            if (!Url.IsLocalUrl(url))
                return RedirectToDefault();

            return Redirect(url);
        }
        public virtual RedirectToActionResult RedirectToDefault()
        {
            return base.RedirectToAction("Index", "Home", new { area = "" });
        }
        public virtual RedirectToActionResult RedirectToNotFound()
        {
            return base.RedirectToAction("NotFound", "Home", new { area = "" });
        }
        public virtual RedirectToActionResult RedirectToUnauthorized()
        {
            return base.RedirectToAction("Unauthorized", "Home", new { area = "" });
        }
        public override RedirectToActionResult RedirectToAction(String actionName, String controllerName, Object routeValues)
        {
            IDictionary<String, Object> values = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            String controller = controllerName ?? (values.ContainsKey("controller") ? values["controller"] as String : null);
            String area = values.ContainsKey("area") ? values["area"] as String : null;
            controller = controller ?? RouteData.Values["controller"] as String;
            area = area ?? RouteData.Values["area"] as String;

            if (!IsAuthorizedFor(actionName, controller, area))
                return RedirectToDefault();

            return base.RedirectToAction(actionName, controllerName, routeValues);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IGlobalizationProvider globalizationProvider = HttpContext.ApplicationServices.GetRequiredService<IGlobalizationProvider>();
            globalizationProvider.CurrentLanguage = globalizationProvider[RouteData.Values["language"] as String];

            AuthorizationProvider = HttpContext.ApplicationServices.GetService<IAuthorizationProvider>();

            CurrentAccountId = User.Id() ?? 0;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            AlertsContainer current = JsonConvert.DeserializeObject<AlertsContainer>(TempData["Alerts"] as String ?? "");
            if (current == null)
                current = Alerts;
            else
                current.Merge(Alerts);

            TempData["Alerts"] = JsonConvert.SerializeObject(current);
        }

        public virtual Boolean IsAuthorizedFor(String action, String controller, String area)
        {
            if (AuthorizationProvider == null) return true;

            return AuthorizationProvider.IsAuthorizedFor(CurrentAccountId, area, controller, action);
        }
    }
}
