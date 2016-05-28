using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            TagBuilder requiredSpan = new TagBuilder("span");
            requiredSpan.Attributes["class"] = "require";

            if (Required == true)
                requiredSpan.InnerHtml.Append("*");

            if (!Required.HasValue && IsRequiredExpression())
                requiredSpan.InnerHtml.Append("*");

            output.Attributes.SetAttribute("for", TagBuilder.CreateSanitizedId(For.Name, Options.IdAttributeDotReplacement));
            output.Content.Append(For.ModelExplorer.Metadata.DisplayName);
            output.Content.AppendHtml(requiredSpan);
        }

        private Boolean IsRequiredExpression()
        {
            if (For.Metadata.ModelType.IsValueType && Nullable.GetUnderlyingType(For.Metadata.ModelType) == null)
                return true;

            return For.Metadata.ValidatorMetadata.Any(validator => validator is RequiredAttribute);
        }
    }
}
