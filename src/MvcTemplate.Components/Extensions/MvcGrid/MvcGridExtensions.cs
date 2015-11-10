using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.DependencyInjection;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Table;
using NonFactors.Mvc.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcTemplate.Components.Extensions
{
    public static class MvcGridExtensions
    {
        public static IGridColumn<T> AddActionLink<T>(this IGridColumnsOf<T> columns, String action, String iconClass) where T : class
        {
            if (!IsAuthorizedToView(columns.Grid, action))
                return new GridColumn<T, String>(columns.Grid, model => "");

            return columns
                .Add(model => GetLink(columns.Grid, model, action, iconClass))
                .Css("action-cell")
                .Encoded(false);
        }

        public static IGridColumn<T> AddDateProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)
        {
            return columns.AddProperty(expression).Formatted("{0:d}");
        }
        public static IGridColumn<T> AddDateProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)
        {
            return columns.AddProperty(expression).Formatted("{0:d}");
        }
        public static IGridColumn<T> AddBooleanProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean>> expression)
        {
            Func<T, Boolean> valueFor = expression.Compile();

            return columns
                .AddProperty(expression)
                .RenderedAs(model =>
                    valueFor(model)
                        ? TableResources.Yes
                        : TableResources.No);
        }
        public static IGridColumn<T> AddBooleanProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean?>> expression)
        {
            Func<T, Boolean?> valueFor = expression.Compile();

            return columns
                .AddProperty(expression)
                .RenderedAs(model =>
                    valueFor(model) != null
                        ? valueFor(model) == true
                            ? TableResources.Yes
                            : TableResources.No
                        : "");
        }
        public static IGridColumn<T> AddDateTimeProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime>> expression)
        {
            return columns.AddProperty(expression).Formatted("{0:g}");
        }
        public static IGridColumn<T> AddDateTimeProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, DateTime?>> expression)
        {
            return columns.AddProperty(expression).Formatted("{0:g}");
        }
        public static IGridColumn<T> AddProperty<T, TProperty>(this IGridColumnsOf<T> columns, Expression<Func<T, TProperty>> expression)
        {
            return columns
                .Add(expression)
                .Css(GetCssClassFor<TProperty>())
                .Titled(ResourceProvider.GetPropertyTitle(expression));
        }

        public static IHtmlGrid<T> ApplyDefaults<T>(this IHtmlGrid<T> grid)
        {
            return grid
                .Pageable(pager => { pager.RowsPerPage = 16; })
                .Named(typeof(T).Name.Replace("View", ""))
                .Empty(TableResources.NoDataFound)
                .Css("table-hover")
                .Filterable()
                .Sortable();
        }

        private static IHtmlContent GetLink<T>(IGrid grid, T model, String action, String iconClass)
        {
            IUrlHelper url = grid.ViewContext.HttpContext.ApplicationServices.GetRequiredService<IUrlHelper>();
            TagBuilder actionTag = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            actionTag.MergeAttribute("href", url.Action(action, GetRouteValuesFor(model)));
            actionTag.AddCssClass(action.ToLower() + "-action");
            icon.AddCssClass(iconClass);

            actionTag.InnerHtml.Append(icon);

            return actionTag;
        }
        private static Boolean IsAuthorizedToView(IGrid grid, String action)
        {
            IAuthorizationProvider provider = grid.ViewContext.HttpContext.ApplicationServices.GetService<IAuthorizationProvider>();
            if (provider == null)
                return true;

            String account = grid.ViewContext.HttpContext.User.Identity.Name;
            String area = grid.ViewContext.RouteData.Values["area"] as String;
            String controller = grid.ViewContext.RouteData.Values["controller"] as String;

            return provider.IsAuthorizedFor(account, area, controller, action);
        }
        private static IDictionary<String, Object> GetRouteValuesFor<T>(T model)
        {
            PropertyInfo keyProperty = typeof(T)
                .GetProperties()
                .FirstOrDefault(property => property.IsDefined(typeof(KeyAttribute), false));

            if (keyProperty == null)
                throw new Exception(String.Format("{0} type does not have a key property.", typeof(T).Name));

            String key = Char.ToLower(keyProperty.Name[0]) + keyProperty.Name.Substring(1);
            IDictionary<String, Object> routeValues = new Dictionary<String, Object>();
            routeValues.Add(key, keyProperty.GetValue(model));

            return routeValues;
        }
        private static String GetCssClassFor<TProperty>()
        {
            Type type = Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty);
            if (type.IsEnum) return "text-left";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "text-right";
                case TypeCode.DateTime:
                    return "text-center";
                default:
                    return "text-left";
            }
        }
    }
}
