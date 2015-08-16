using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using MvcTemplate.Components.Html;
using System;
using System.Collections.Generic;
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
            JsTree tree = ExpressionMetadataProvider.FromStringExpression(For.Name, ViewContext.ViewData, Html.MetadataProvider).Model as JsTree;
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
}
