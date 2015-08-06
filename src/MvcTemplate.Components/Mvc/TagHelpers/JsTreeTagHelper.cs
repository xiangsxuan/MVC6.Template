using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using MvcTemplate.Components.Html;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MvcTemplate.Components.Mvc
{
    [TargetElement("div", Attributes = "jstree-for")]
    public class JsTreeTagHelper : TagHelper
    {
        [HtmlAttributeName("jstree-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        public IHtmlHelper Html { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public JsTreeTagHelper(IHtmlHelper html)
        {
            Html = html;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            JsTree tree = ExpressionMetadataProvider2.FromStringExpression(For.Name, ViewContext.ViewData, Html.MetadataProvider).Model as JsTree;
            output.Attributes["class"] = (output.Attributes["class"]?.Value + " js-tree").Trim();

            output.Content.SetContent(JsTreeFor(For.Name + ".SelectedIds", tree));
        }

        private static String JsTreeFor(String name, JsTree jsTree)
        {
            TagBuilder ids = new TagBuilder("div");
            ids.AddCssClass("js-tree-view-ids");

            StringBuilder hiddenInputs = new StringBuilder();
            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("name", name);

            foreach (String id in jsTree.SelectedIds)
            {
                input.MergeAttribute("value", id, true);
                hiddenInputs.Append(input.ToString(TagRenderMode.SelfClosing));
            }

            ids.InnerHtml = hiddenInputs.ToString();
            TagBuilder tree = new TagBuilder("div");
            tree.MergeAttribute("for", name);
            tree.AddCssClass("js-tree-view");
            AddNodes(tree, jsTree.Nodes);

            return ids + tree.ToString();
        }
        private static void AddNodes(TagBuilder root, IList<JsTreeNode> nodes)
        {
            if (nodes.Count == 0) return;

            StringBuilder leafBuilder = new StringBuilder();
            TagBuilder branch = new TagBuilder("ul");

            foreach (JsTreeNode treeNode in nodes)
            {
                TagBuilder node = new TagBuilder("li");
                node.MergeAttribute("id", treeNode.Id);
                node.InnerHtml = treeNode.Title;

                AddNodes(node, treeNode.Nodes);
                leafBuilder.Append(node);
            }

            branch.InnerHtml = leafBuilder.ToString();
            root.InnerHtml += branch.ToString();
        }
    }

    public static class ExpressionMetadataProvider2
    {
        public static ModelExplorer FromStringExpression(
            string expression,
            ViewDataDictionary viewData,
            IModelMetadataProvider metadataProvider)
        {
            var viewDataInfo = ViewDataEvaluator.Eval(viewData, expression);
            if (viewDataInfo == null)
            {
                // Try getting a property from ModelMetadata if we couldn't find an answer in ViewData
                var propertyExplorer = viewData.ModelExplorer.GetExplorerForProperty(expression);
                if (propertyExplorer != null)
                {
                    return propertyExplorer;
                }
            }

            if (viewDataInfo != null)
            {
                if (viewDataInfo.Container == viewData &&
                    viewDataInfo.Value == viewData.Model &&
                    string.IsNullOrEmpty(expression))
                {
                    // Nothing for empty expression in ViewData and ViewDataEvaluator just returned the model. Handle
                    // using FromModel() for its object special case.
                    return FromModel(viewData, metadataProvider);
                }

                ModelExplorer containerExplorer = viewData.ModelExplorer;
                if (viewDataInfo.Container != null)
                {
                    containerExplorer = metadataProvider.GetModelExplorerForType(
                        viewDataInfo.Container.GetType(),
                        viewDataInfo.Container);
                }

                if (viewDataInfo.PropertyInfo != null)
                {
                    // We've identified a property access, which provides us with accurate metadata.
                    var containerType = viewDataInfo.Container?.GetType() ?? viewDataInfo.PropertyInfo.DeclaringType;
                    var containerMetadata = metadataProvider.GetMetadataForType(viewDataInfo.Container.GetType());
                    var propertyMetadata = containerMetadata.Properties[viewDataInfo.PropertyInfo.Name];

                    Func<object, object> modelAccessor = (ignore) => viewDataInfo.Value;
                    return containerExplorer.GetExplorerForExpression(propertyMetadata, modelAccessor);
                }
                else if (viewDataInfo.Value != null)
                {
                    // We have a value, even though we may not know where it came from.
                    var valueMetadata = metadataProvider.GetMetadataForType(viewDataInfo.Value.GetType());
                    return containerExplorer.GetExplorerForExpression(valueMetadata, viewDataInfo.Value);
                }
            }

            // Treat the expression as string if we don't find anything better.
            var stringMetadata = metadataProvider.GetMetadataForType(typeof(string));
            return viewData.ModelExplorer.GetExplorerForExpression(stringMetadata, modelAccessor: null);
        }

        private static ModelExplorer FromModel(
            ViewDataDictionary viewData,
            IModelMetadataProvider metadataProvider)
        {
            if (viewData.ModelMetadata.ModelType == typeof(object))
            {
                // Use common simple type rather than object so e.g. Editor() at least generates a TextBox.
                var model = viewData.Model == null ? null : Convert.ToString(viewData.Model, CultureInfo.CurrentCulture);
                return metadataProvider.GetModelExplorerForType(typeof(string), model);
            }
            else
            {
                return viewData.ModelExplorer;
            }
        }
    }
}
