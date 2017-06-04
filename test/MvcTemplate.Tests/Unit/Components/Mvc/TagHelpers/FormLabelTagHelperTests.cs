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
        private FormLabelTagHelper helper;
        private TagHelperOutput output;
        private ModelMetadata metadata;

        public FormLabelTagHelperTests()
        {
            metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(String)));
            IOptions<HtmlHelperOptions> options = Substitute.For<IOptions<HtmlHelperOptions>>();
            options.Value.Returns(new HtmlHelperOptions { IdAttributeDotReplacement = "___" });
            helper = new FormLabelTagHelper(options);
            metadata.DisplayName.Returns("Title");

            TagHelperAttribute[] attributes = { new TagHelperAttribute("for", "Test") };
            output = new TagHelperOutput("label", new TagHelperAttributeList(attributes), (useCache, encoder) => null);
            helper.For = new ModelExpression("Total.Sum", new ModelExplorer(new EmptyModelMetadataProvider(), metadata, null));
        }

        #region Process(TagHelperContext context, TagHelperOutput output)

        [Theory]
        [InlineData(true, true, "*")]
        [InlineData(true, false, "")]
        [InlineData(false, true, "*")]
        [InlineData(false, false, "")]
        public void Process_Label(Boolean metadataRequired, Boolean helperRequired, String required)
        {
            metadata.IsRequired.Returns(metadataRequired);
            helper.Required = helperRequired;

            helper.Process(null, output);

            Assert.Equal("Total___Sum", output.Attributes["for"].Value);
            Assert.Equal($"Title<span class=\"require\">{required}</span>", output.Content.GetContent());
        }

        #endregion
    }
}
