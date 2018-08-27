using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcTemplate.Components.Extensions;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    [HtmlTargetElement("div", Attributes = "mvc-tree-for")]
    public class MvcTreeTagHelper : TagHelper
    {
        [HtmlAttributeName("hide-depth")]
        public Int32? HideDepth { get; set; }

        [HtmlAttributeName("mvc-tree-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            MvcTree tree = For.Model as MvcTree;

            output.Content.AppendHtml(IdsFor(tree));
            output.Content.AppendHtml(ViewFor(tree));

            output.Attributes.SetAttribute("data-for", For.Name + ".SelectedIds");
            output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " mvc-tree").Trim());
        }

        private TagBuilder IdsFor(MvcTree model)
        {
            String name = For.Name + ".SelectedIds";
            TagBuilder ids = new TagBuilder("div");
            ids.AddCssClass("mvc-tree-ids");

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
        private TagBuilder ViewFor(MvcTree model)
        {
            TagBuilder root = new TagBuilder("ul");
            root.AddCssClass("mvc-tree-view");

            return Build(model, root, model.Nodes, 1);
        }
        private TagBuilder Build(MvcTree model, TagBuilder branch, List<MvcTreeNode> nodes, Int32 depth)
        {
            foreach (MvcTreeNode node in nodes)
            {
                TagBuilder item = new TagBuilder("li");
                item.InnerHtml.AppendHtml("<i></i>");

                if (node.Id != null) {
                    if (model.SelectedIds.Contains(node.Id.Value))
                        item.AddCssClass("mvc-tree-checked");

                    item.Attributes["data-id"] = node.Id.ToString();
                }

                TagBuilder anchor = new TagBuilder("a");
                anchor.InnerHtml.Append(node.Title);
                anchor.Attributes["href"] = "#";

                item.InnerHtml.AppendHtml(anchor);

                if (node.Children.Count > 0)
                {
                    item.AddCssClass("mvc-tree-branch");

                    if (HideDepth <= depth)
                        item.AddCssClass("mvc-tree-collapsed");

                    item.InnerHtml.AppendHtml(Build(model, new TagBuilder("ul"), node.Children, depth + 1));
                }

                branch.InnerHtml.AppendHtml(item);
            }

            return branch;
        }
    }
}
