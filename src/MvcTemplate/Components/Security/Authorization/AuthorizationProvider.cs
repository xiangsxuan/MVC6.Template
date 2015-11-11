using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Components.Security
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private Type[] ControllerTypes { get; }
        private Dictionary<String, IEnumerable<Privilege>> Cache { get; set; }

        public AuthorizationProvider(Assembly controllersAssembly)
        {
            ControllerTypes = controllersAssembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).ToArray();

            Refresh();
        }

        public Boolean IsAuthorizedFor(String accountId, String area, String controller, String action)
        {
            Type authorizedController = GetControllerType(area, controller);
            MethodInfo actionInfo = GetMethod(authorizedController, action);
            AuthorizeAsAttribute authorizeAs = GetAuthorizeAs(actionInfo);
            if (String.IsNullOrEmpty(area)) area = null;

            if (authorizeAs != null)
                return IsAuthorizedFor(accountId, authorizeAs.Area ?? area, authorizeAs.Controller ?? controller, authorizeAs.Action);

            if (AllowsUnauthorized(authorizedController, actionInfo))
                return true;

            if (!Cache.ContainsKey(accountId ?? ""))
                return true;

            return Cache[accountId]
                .Any(privilege =>
                    String.Equals(privilege.Area, area, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(privilege.Action, action, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(privilege.Controller, controller, StringComparison.OrdinalIgnoreCase));
        }

        public void Refresh()
        {
            Cache = new Dictionary<String, IEnumerable<Privilege>>();
        }

        private Boolean AllowsUnauthorized(Type authorizedControllerType, MethodInfo method)
        {
            if (method.IsDefined(typeof(AuthorizeAttribute), false)) return false;
            if (method.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

            while (authorizedControllerType != typeof(Controller))
            {
                if (authorizedControllerType.IsDefined(typeof(AuthorizeAttribute), false)) return false;
                if (authorizedControllerType.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
                if (authorizedControllerType.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

                authorizedControllerType = authorizedControllerType.BaseType;
            }

            return true;
        }
        private Type GetControllerType(String area, String controller)
        {
            String controllerType = controller + "Controller";
            IEnumerable<Type> controllers = ControllerTypes
                .Where(type => type.Name.Equals(controllerType, StringComparison.OrdinalIgnoreCase));

            if (String.IsNullOrEmpty(area))
                controllers = controllers.Where(type =>
                    !type.IsDefined(typeof(AreaAttribute), false));
            else
                controllers = controllers.Where(type =>
                    type.IsDefined(typeof(AreaAttribute), false) &&
                    String.Equals(type.GetCustomAttribute<AreaAttribute>(false).RouteValue, area, StringComparison.OrdinalIgnoreCase));

            return controllers.Single();
        }
        private MethodInfo GetMethod(Type controller, String action)
        {
            MethodInfo[] methods = controller
                .GetMethods()
                .Where(method =>
                    (
                        !method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.Name.ToLowerInvariant() == action.ToLowerInvariant()
                    )
                    ||
                    (
                        method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.GetCustomAttribute<ActionNameAttribute>(false).Name.ToLowerInvariant() == action.ToLowerInvariant()
                    ))
                .ToArray();

            MethodInfo getMethod = methods.FirstOrDefault(method => method.IsDefined(typeof(HttpGetAttribute), false));
            if (getMethod != null)
                return getMethod;

            if (methods.Length == 0)
                throw new Exception($"'{controller.Name}' does not have '{action}' action.");

            return methods[0];
        }
        private AuthorizeAsAttribute GetAuthorizeAs(MemberInfo action)
        {
            if (!action.IsDefined(typeof(AuthorizeAsAttribute), false))
                return null;

            return action.GetCustomAttribute<AuthorizeAsAttribute>(false);
        }
    }
}
