using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Shared;
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
            if (!IsAuthorizedTo(columns.Grid.ViewContext, action))
                return new GridColumn<T, String>(columns.Grid, model => "");

            return columns
                .Add(model => GetLink(columns.Grid.ViewContext, model, action, iconClass))
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
                .RenderedAs(model => valueFor(model) ? Strings.Yes : Strings.No);
        }
        public static IGridColumn<T> AddBooleanProperty<T>(this IGridColumnsOf<T> columns, Expression<Func<T, Boolean?>> expression)
        {
            Func<T, Boolean?> valueFor = expression.Compile();

            return columns
                .AddProperty(expression)
                .RenderedAs(model =>
                    valueFor(model) != null
                        ? valueFor(model) == true
                            ? Strings.Yes
                            : Strings.No
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
                .Css((grid.Grid.CssClasses + " table-hover").TrimStart())
                .Pageable(pager => { pager.RowsPerPage = 16; })
                .Empty(Strings.NoDataFound)
                .Filterable()
                .Sortable();
        }

        private static IHtmlContent GetLink<T>(ViewContext context, T model, String action, String iconClass)
        {
            TagBuilder anchor = new TagBuilder("a");
            anchor.AddCssClass(action.ToLower() + "-action");
            anchor.Attributes["href"] = new UrlHelper(context).Action(action, GetRouteFor(model));

            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass(iconClass);

            anchor.InnerHtml.AppendHtml(icon);

            return anchor;
        }
        private static Boolean IsAuthorizedTo(ViewContext view, String action)
        {
            IAuthorizationProvider authorization = view.HttpContext.RequestServices.GetService<IAuthorizationProvider>();
            if (authorization == null)
                return true;

            Int32? account = view.HttpContext.User.Id();
            String area = view.RouteData.Values["area"] as String;
            String controller = view.RouteData.Values["controller"] as String;

            return authorization.IsAuthorizedFor(account, area, controller, action);
        }
        private static IDictionary<String, Object> GetRouteFor<T>(T model)
        {
            PropertyInfo key = typeof(T)
                .GetProperties()
                .FirstOrDefault(property => property.IsDefined(typeof(KeyAttribute), false));

            if (key == null)
                throw new Exception(typeof(T).Name + " type does not have a key property.");

            return new Dictionary<String, Object> { [key.Name] = key.GetValue(model) };
        }
        private static String GetCssClassFor<TProperty>()
        {
            Type type = Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty);
            if (type.IsEnum)
                return "text-left";

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
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                    return "text-center";
                default:
                    return "text-left";
            }
        }
    }
}
