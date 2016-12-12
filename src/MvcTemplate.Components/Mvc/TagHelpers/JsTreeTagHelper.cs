using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcTemplate.Components.Extensions;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    [HtmlTargetElement("div", Attributes = "jstree-for")]
    public class JsTreeTagHelper : TagHelper
    {
        [HtmlAttributeName("jstree-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            JsTree tree = For.Model as JsTree;

            output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " js-tree").Trim());
            output.Content.AppendHtml(HiddenIdsFor(tree));
            output.Content.AppendHtml(JsTreeFor(tree));
        }

        private void Add(TagBuilder root, List<JsTreeNode> nodes)
        {
            TagBuilder branch = new TagBuilder("ul");
            foreach (JsTreeNode node in nodes)
            {
                TagBuilder item = new TagBuilder("li");
                item.InnerHtml.Append(node.Title);
                String id = node.Id.ToString();
                item.Attributes["id"] = id;

                Add(item, node.Nodes);
                branch.InnerHtml.AppendHtml(item);
            }

            if (nodes.Count > 0)
                root.InnerHtml.AppendHtml(branch);
        }
        private TagBuilder HiddenIdsFor(JsTree model)
        {
            String name = For.Name + ".SelectedIds";
            TagBuilder ids = new TagBuilder("div");
            ids.AddCssClass("js-tree-view-ids");

            foreach (Int32 id in model.SelectedIds)
            {
                TagBuilder input = new TagBuilder("input");
                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.Attributes["value"] = id.ToString();
                input.Attributes["type"] = "hidden";
                input.Attributes["name"] = name;

                ids.InnerHtml.AppendHtml(input);
            }

            return ids;
        }
        private TagBuilder JsTreeFor(JsTree model)
        {
            String name = For.Name + ".SelectedIds";
            TagBuilder tree = new TagBuilder("div");
            tree.AddCssClass("js-tree-view");
            tree.Attributes["for"] = name;

            Add(tree, model.Nodes);

            return tree;
        }
    }
}
