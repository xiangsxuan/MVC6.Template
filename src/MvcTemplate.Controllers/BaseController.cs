using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Controllers
{
    public abstract class BaseController : Controller
    {
        public IAuthorizationProvider AuthorizationProvider
        {
            get;
            protected set;
        }
        public virtual String CurrentAccountId
        {
            get
            {
                return User.Identity.Name;
            }
        }
        public AlertsContainer Alerts
        {
            get;
            protected set;
        }

        protected BaseController()
        {
            AuthorizationProvider = Authorization.Provider;
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
            return RedirectToAction("", "", new { area = "" });
        }
        public virtual RedirectToActionResult RedirectToNotFound()
        {
            return RedirectToAction("NotFound", "Home", new { area = "" });
        }
        public virtual RedirectToActionResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", "Home", new { area = "" });
        }
        public virtual RedirectToActionResult RedirectIfAuthorized(String action)
        {
            if (!IsAuthorizedFor(action))
                return RedirectToDefault();

            return RedirectToAction(action);
        }
        public virtual RedirectToActionResult RedirectIfAuthorized(String action, Object routeValues)
        {
            IDictionary<String, Object> values = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            String controller = (values["controller"] ?? RouteData.Values["controller"]) as String;
            String area = (values["area"] ?? RouteData.Values["area"]) as String;

            if (!IsAuthorizedFor(area, controller, action))
                return RedirectToDefault();

            return RedirectToAction(action, routeValues);
        }

        public virtual Boolean IsAuthorizedFor(String action)
        {
            String area = (String)RouteData.Values["area"];
            String controller = (String)RouteData.Values["controller"];

            return IsAuthorizedFor(area, controller, action);
        }
        public virtual Boolean IsAuthorizedFor(String area, String controller, String action)
        {
            if (AuthorizationProvider == null) return true;

            return AuthorizationProvider.IsAuthorizedFor(CurrentAccountId, area, controller, action);
        }
    }
}
