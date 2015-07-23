using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class BootstrapExtensions
    {
        public static HtmlString FormLabelFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder requiredSpan = new TagBuilder("span");
            TagBuilder label = new TagBuilder("label");
            requiredSpan.AddCssClass("require");

            if (expression.IsRequired())
                requiredSpan.InnerHtml = "*";

            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression), "_"));
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression) + requiredSpan;

            return new HtmlString(label.ToString());
        }

        public static HtmlString FormTextBoxFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormTextBoxFor(expression, null, null);
        }
        public static HtmlString FormTextBoxFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)
        {
            return html.FormTextBoxFor(expression, format, null);
        }
        public static HtmlString FormTextBoxFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            return html.FormTextBoxFor(expression, null, htmlAttributes);
        }
        public static HtmlString FormTextBoxFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)
        {
            IDictionary<String, Object> attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control");

            return html.TextBoxFor(expression, format, attributes);
        }

        public static HtmlString FormPasswordFor<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            return html.PasswordFor(expression, new { @class = "form-control", autocomplete = "off" });
        }

        public static HtmlString FormTextAreaFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            IDictionary<String, Object> attributes = FormHtmlAttributes(expression, new { rows = 6 }, "form-control");

            return html.TextAreaFor(expression, attributes);
        }

        public static HtmlString FormDatePickerFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormDatePickerFor(expression, null);
        }
        public static HtmlString FormDatePickerFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            IDictionary<String, Object> attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control datepicker");

            return html.TextBoxFor(expression, "{0:d}", attributes);
        }

        public static HtmlString FormDateTimePickerFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormDateTimePickerFor(expression, null);
        }
        public static HtmlString FormDateTimePickerFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            IDictionary<String, Object> attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control datetimepicker");

            return html.TextBoxFor(expression, "{0:g}", attributes);
        }

        private static IDictionary<String, Object> FormHtmlAttributes(LambdaExpression expression, Object attributes, String cssClass)
        {
            IDictionary<String, Object> htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            if (htmlAttributes.ContainsKey("class"))
                htmlAttributes["class"] = (cssClass + " " + htmlAttributes["class"]).Trim();
            else
                htmlAttributes.Add("class", cssClass);

            if (!htmlAttributes.ContainsKey("autocomplete"))
                htmlAttributes.Add("autocomplete", "off");

            if (htmlAttributes.ContainsKey("readonly"))
                return htmlAttributes;

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null && memberExpression.Member.IsDefined(typeof(EditableAttribute), false))
            {
                EditableAttribute editable = memberExpression.Member.GetCustomAttribute<EditableAttribute>(false);
                if (!editable.AllowEdit) htmlAttributes.Add("readonly", "readonly");
            }

            return htmlAttributes;
        }
        private static Boolean IsRequired(this LambdaExpression expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression.");

            if (!AllowsNullValues(expression.ReturnType))
                return true;

            return memberExpression.Member.IsDefined(typeof(RequiredAttribute), false);
        }
        private static Boolean AllowsNullValues(Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }
    }
}
