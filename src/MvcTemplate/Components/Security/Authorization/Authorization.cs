using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Components.Security
{
    public class Authorization : IAuthorization
    {
        private IServiceProvider Services { get; }
        private Dictionary<String, String> Required { get; }
        private Dictionary<String, MethodInfo> Actions { get; }
        private Dictionary<Int32, HashSet<String>> Permissions { get; set; }

        public Authorization(Assembly controllers, IServiceProvider services)
        {
            BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
            Actions = new Dictionary<String, MethodInfo>(StringComparer.OrdinalIgnoreCase);
            Required = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
            Permissions = new Dictionary<Int32, HashSet<String>>();
            Services = services;

            foreach (Type controller in controllers.GetTypes().Where(IsController))
                foreach (MethodInfo method in controller.GetMethods(flags).Where(IsAction))
                    Actions[PermissionFor(method)] = method;

            foreach (String permission in Actions.Keys)
                if (RequiredAuthorizationFor(permission) is String required)
                    Required[permission] = required;

            Refresh();
        }

        public Boolean IsGrantedFor(Int32? accountId, String area, String controller, String action)
        {
            String permission = (area + "/" + controller + "/" + action).ToLower();
            if (!Required.ContainsKey(permission))
                return true;

            return Permissions.ContainsKey(accountId ?? 0) && Permissions[accountId.Value].Contains(Required[permission]);
        }

        public void Refresh()
        {
            using (IUnitOfWork unitOfWork = Services.GetRequiredService<IUnitOfWork>())
            {
                Permissions = unitOfWork
                    .Select<Account>()
                    .Where(account =>
                        !account.IsLocked &&
                        account.RoleId != null)
                    .Select(account => new
                    {
                        Id = account.Id,
                        Permissions = account
                            .Role
                            .Permissions
                            .Select(role => role.Permission)
                            .Select(permission => (permission.Area ?? "") + "/" + permission.Controller + "/" + permission.Action)
                    })
                    .ToDictionary(
                        account => account.Id,
                        account => new HashSet<String>(account.Permissions, StringComparer.OrdinalIgnoreCase));
            }
        }

        private Boolean RequiresAuthorizationFor(String action)
        {
            MethodInfo method = Actions[action];
            if (method.IsDefined(typeof(AuthorizeAttribute), false))
                return true;

            if (method.IsDefined(typeof(AllowAnonymousAttribute), false))
                return false;

            if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false))
                return false;

            Type controller = method.DeclaringType;
            while (controller != typeof(Controller))
            {
                if (controller.IsDefined(typeof(AuthorizeAttribute), false))
                    return true;

                if (controller.IsDefined(typeof(AllowAnonymousAttribute), false))
                    return false;

                if (controller.IsDefined(typeof(AllowUnauthorizedAttribute), false))
                    return false;

                controller = controller.BaseType;
            }

            return false;
        }
        private String RequiredAuthorizationFor(String action)
        {
            String[] path = action.Split('/');
            AuthorizeAsAttribute auth = Actions[action].GetCustomAttribute<AuthorizeAsAttribute>(false);
            String asAction = $"{auth?.Area ?? path[0]}/{auth?.Controller ?? path[1]}/{auth?.Action ?? path[2]}";

            if (action != asAction)
                return RequiredAuthorizationFor(asAction);

            return RequiresAuthorizationFor(action) ? action : null;
        }
        private String PermissionFor(MethodInfo method)
        {
            String controller = method.DeclaringType.Name.Substring(0, method.DeclaringType.Name.Length - 10);
            String action = method.GetCustomAttribute<ActionNameAttribute>(false)?.Name ?? method.Name;
            String area = method.DeclaringType.GetCustomAttribute<AreaAttribute>(false)?.RouteValue;

            return $"{area}/{controller}/{action}";
        }
        private Boolean IsAction(MethodInfo method)
        {
            return !method.IsSpecialName && !method.IsDefined(typeof(NonActionAttribute));
        }
        private Boolean IsController(Type type)
        {
            return type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                typeof(Controller).IsAssignableFrom(type) &&
                !type.IsAbstract &&
                type.IsPublic;
        }
    }
}
