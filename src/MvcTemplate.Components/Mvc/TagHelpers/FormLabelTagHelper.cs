using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;

namespace MvcTemplate.Components.Mvc
{
    [HtmlTargetElement("label", Attributes = "for")]
    public class FormLabelTagHelper : TagHelper
    {
        public Boolean? Required { get; set; }
        public ModelExpression For { get; set; }
        private HtmlHelperOptions Options { get; }

        public FormLabelTagHelper(IOptions<HtmlHelperOptions> options)
        {
            Options = options.Value;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder require = new TagBuilder("span");
            require.Attributes["class"] = "require";

            if (Required == true)
                require.InnerHtml.Append("*");

            if (Required == null && For.Metadata.IsRequired && For.Metadata.ModelType != typeof(Boolean))
                require.InnerHtml.Append("*");

            output.Attributes.SetAttribute("for", TagBuilder.CreateSanitizedId(For.Name, Options.IdAttributeDotReplacement));
            output.Content.Append(For.ModelExplorer.Metadata.DisplayName);
            output.Content.AppendHtml(require);
        }
    }
}
