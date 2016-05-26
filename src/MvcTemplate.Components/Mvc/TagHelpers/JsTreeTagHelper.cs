using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcTemplate.Components.Html;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    [HtmlTargetElement("div", Attributes = "jstree-for")]
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
            output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " js-tree").Trim());

            output.Content.AppendHtml(HiddenIdsFor(tree));
            output.Content.AppendHtml(JsTreeFor(tree));
        }

        private TagBuilder HiddenIdsFor(JsTree jsTree)
        {
            String name = For.Name + ".SelectedIds";
            TagBuilder ids = new TagBuilder("div");
            ids.AddCssClass("js-tree-view-ids");

            foreach (Int32 id in jsTree.SelectedIds)
            {
                TagBuilder input = new TagBuilder("input");

                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.MergeAttribute("value", id.ToString());
                input.MergeAttribute("type", "hidden");
                input.MergeAttribute("name", name);

                ids.InnerHtml.AppendHtml(input);
            }

            return ids;
        }
        private TagBuilder JsTreeFor(JsTree jsTree)
        {
            String name = For.Name + ".SelectedIds";
            TagBuilder tree = new TagBuilder("div");
            tree.MergeAttribute("for", name);
            tree.AddCssClass("js-tree-view");
            AddNodes(tree, jsTree.Nodes);

            return tree;
        }
        private void AddNodes(TagBuilder root, IList<JsTreeNode> nodes)
        {
            if (nodes.Count == 0) return;

            TagBuilder branch = new TagBuilder("ul");

            foreach (JsTreeNode treeNode in nodes)
            {
                TagBuilder node = new TagBuilder("li");
                node.InnerHtml.Append(treeNode.Title);
                String id = treeNode.Id.ToString();
                node.MergeAttribute("id", id);

                AddNodes(node, treeNode.Nodes);
                branch.InnerHtml.AppendHtml(node);
            }

            root.InnerHtml.AppendHtml(branch);
        }
    }
}
