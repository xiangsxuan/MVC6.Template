using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

namespace MvcTemplate.Resources
{
    public static class Resource
    {
        private static Dictionary<String, ResourceManager> Properties { get; }
        private static Dictionary<String, ResourceManager> Resources { get; }

        static Resource()
        {
            Resources = new Dictionary<String, ResourceManager>();
            Properties = new Dictionary<String, ResourceManager>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
                if (type.GetProperty("ResourceManager", flags) is PropertyInfo property)
                {
                    ResourceManager manager = property.GetValue(null) as ResourceManager;
                    manager.IgnoreCase = true;

                    if (type.FullName.StartsWith("MvcTemplate.Resources.Views") && type.FullName.EndsWith("View.Titles"))
                        Properties.Add(type.Namespace.Split('.').Last(), manager);
                    else
                        Resources.Add(type.FullName, manager);
                }
            }
        }

        public static String ForPage(IDictionary<String, Object> values)
        {
            String area = values["area"] as String;
            String action = values["action"] as String;
            String controller = values["controller"] as String;

            return Localized("MvcTemplate.Resources.Shared.Pages", area + controller + action);
        }

        public static String ForSiteMap(String area, String controller, String action)
        {
            return Localized("MvcTemplate.Resources.SiteMap.Titles", area + controller + action);
        }

        public static String ForPermission(String area)
        {
            return Localized("MvcTemplate.Resources.Permissions.Area.Titles", area ?? "");
        }
        public static String ForPermission(String area, String controller)
        {
            return Localized("MvcTemplate.Resources.Permissions.Controller.Titles", area + controller);
        }
        public static String ForPermission(String area, String controller, String action)
        {
            return Localized("MvcTemplate.Resources.Permissions.Action.Titles", area + controller + action);
        }

        public static String ForProperty<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return ForProperty(expression.Body);
        }
        public static String ForProperty(String view, String name)
        {
            if (LocalizedProperty(view, name) is String title)
                return title;

            String[] properties = SplitCamelCase(name);
            for (Int32 skipped = 0; skipped < properties.Length; skipped++)
            {
                for (Int32 viewSize = 1; viewSize < properties.Length - skipped; viewSize++)
                {
                    String joinedView = String.Concat(properties.Skip(skipped).Take(viewSize)) + "View";
                    String joinedProperty = String.Concat(properties.Skip(viewSize + skipped));

                    if (LocalizedProperty(joinedView, joinedProperty) is String joinedTitle)
                        return joinedTitle;
                }
            }

            return null;
        }
        public static String ForProperty(Type view, String name)
        {
            return ForProperty(view.Name, name ?? "");
        }
        public static String ForProperty(Expression expression)
        {
            return expression is MemberExpression member ? ForProperty(member.Expression.Type, member.Member.Name) : null;
        }

        private static String LocalizedProperty(String type, String name)
        {
            return Properties.ContainsKey(type) ? Properties[type].GetString(name) : null;
        }
        private static String Localized(String type, String key)
        {
            return Resources.ContainsKey(type) ? Resources[type].GetString(key) : null;
        }
        private static String[] SplitCamelCase(String value)
        {
            return Regex.Split(value, "(?<!^)(?=[A-Z])");
        }
    }
}
