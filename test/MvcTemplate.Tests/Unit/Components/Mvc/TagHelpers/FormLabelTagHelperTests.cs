using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class FormLabelTagHelperTests
    {
        private FormLabelTagHelper helper;
        private TagHelperOutput output;

        public FormLabelTagHelperTests()
        {
            output = new TagHelperOutput("label", new TagHelperAttributeList());
            helper = new FormLabelTagHelper();
        }

        #region Method: Process(TagHelperContext context, TagHelperOutput output)

        [Fact]
        public void Process_SetsForAttributeValue()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "NotRequired", typeof(TagHelperModel)));
            helper.For = new ModelExpression("Relation.NotRequired", new ModelExplorer(null, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");

            helper.Process(null, output);

            Object expected = TagBuilder.CreateSanitizedId(helper.For.Name, "_");
            Object actual = output.Attributes["for"].Value;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_OverridesForAttributeValue()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "NotRequired", typeof(TagHelperModel)));
            helper.For = new ModelExpression("Relation.NotRequired", new ModelExplorer(null, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";

            helper.Process(null, output);

            Object expected = TagBuilder.CreateSanitizedId(helper.For.Name, "_");
            Object actual = output.Attributes["for"].Value;

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void Process_AddsRequiredSymbolOnRequiredProperties()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "Required", typeof(TagHelperModel)));
            helper.For = new ModelExpression("Required", new ModelExplorer(null, metadata, null));
            metadata.ValidatorMetadata.Returns(new[] { new RequiredAttribute() });
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");

            helper.Process(null, output);

            String expected = "Title<span class=\"require\">*</span>";
            String actual = output.Content.GetContent();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_AddsRequiredSymbolOnValueTypes()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(Int64), "RequiredValue", typeof(TagHelperModel)));
            helper.For = new ModelExpression("RequiredValue", new ModelExplorer(null, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");

            helper.Process(null, output);

            String expected = "Title<span class=\"require\">*</span>";
            String actual = output.Content.GetContent();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_DoesNotAddRequiredSymbolOnNotRequiredProperties()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "NotRequired", typeof(TagHelperModel)));
            helper.For = new ModelExpression("NotRequired", new ModelExplorer(null, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");

            helper.Process(null, output);

            String expected = "Title<span class=\"require\"></span>";
            String actual = output.Content.GetContent();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_DoesNotAddRequiredSymbolOnNullableValueTypes()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(Int64?), "NotRequiredNullableValue", typeof(TagHelperModel)));
            helper.For = new ModelExpression("NotRequiredNullableValue", new ModelExplorer(null, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");

            helper.Process(null, output);

            String expected = "Title<span class=\"require\"></span>";
            String actual = output.Content.GetContent();

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
