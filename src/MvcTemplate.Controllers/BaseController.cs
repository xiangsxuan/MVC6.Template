using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Framework.DependencyInjection;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        public IAuthorizationProvider AuthorizationProvider { get; protected set; }
        public virtual String CurrentAccountId => User.Identity.Name;
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
            base.OnActionExecuting(context);

            AuthorizationProvider = HttpContext.ApplicationServices.GetService<IAuthorizationProvider>();
        }

        public virtual Boolean IsAuthorizedFor(String action, String controller, String area)
        {
            if (AuthorizationProvider == null) return true;

            return AuthorizationProvider.IsAuthorizedFor(CurrentAccountId, area, controller, action);
        }
    }
}
