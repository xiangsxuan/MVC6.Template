using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class FormLabelTagHelperTests
    {
        #region Process(TagHelperContext context, TagHelperOutput output)

        [Theory]
        [InlineData(typeof(String), true, null, "*")]
        [InlineData(typeof(String), true, true, "*")]
        [InlineData(typeof(String), true, false, "")]
        [InlineData(typeof(String), false, null, "")]
        [InlineData(typeof(String), false, true, "*")]
        [InlineData(typeof(String), false, false, "")]
        [InlineData(typeof(Boolean), true, null, "")]
        [InlineData(typeof(Boolean), true, true, "*")]
        [InlineData(typeof(Boolean), true, false, "")]
        public void Process_Label(Type type, Boolean metadataRequired, Boolean? required, String require)
        {
            ModelMetadata metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(type));
            IOptions<HtmlHelperOptions> options = Substitute.For<IOptions<HtmlHelperOptions>>();
            options.Value.Returns(new HtmlHelperOptions { IdAttributeDotReplacement = "___" });
            TagHelperAttribute[] attributes = { new TagHelperAttribute("for", "Test") };
            FormLabelTagHelper helper = new FormLabelTagHelper(options);

            TagHelperOutput output = new TagHelperOutput("label", new TagHelperAttributeList(attributes), (useCache, encoder) => null);
            helper.For = new ModelExpression("Total.Sum", new ModelExplorer(new EmptyModelMetadataProvider(), metadata, null));
            metadata.IsRequired.Returns(metadataRequired);
            metadata.DisplayName.Returns("Title");
            helper.Required = required;

            helper.Process(null, output);

            Assert.Equal("Test", output.Attributes["for"].Value);
            Assert.Equal($"<span class=\"require\">{require}</span>", output.Content.GetContent());
        }

        #endregion
    }
}
