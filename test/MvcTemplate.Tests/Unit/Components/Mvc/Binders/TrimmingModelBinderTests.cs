using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class TrimmingModelBinderTests
    {
        private TrimmingModelBinder binder;
        private ModelBindingContext context;

        public TrimmingModelBinderTests()
        {
            binder = new TrimmingModelBinder();
            context = new ModelBindingContext();
            context.ModelState = new ModelStateDictionary();
            context.ValueProvider = Substitute.For<IValueProvider>();
        }

        #region Method: BindModelAsync(ModelBindingContext context)

        [Fact]
        public void BindModelAsync_NotString_ReturnsNoResult()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(BindersModel));
            context.ModelMetadata = metadata;

            ModelBindingResult actual = binder.BindModelAsync(context).Result;
            ModelBindingResult expected = ModelBindingResult.NoResult;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BindModelAsync_NoValue_ReturnsNoResult()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(String));
            context.ValueProvider.GetValue(context.ModelName).Returns(ValueProviderResult.None);
            context.ModelMetadata = metadata;

            ModelBindingResult actual = binder.BindModelAsync(context).Result;
            ModelBindingResult expected = ModelBindingResult.NoResult;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BindModelAsync_NotTrimmed_ReturnsNoResult()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "NotTrimmed");
            context.ValueProvider.GetValue("Model.NotTrimmed").Returns(new ValueProviderResult(" Value "));
            context.ModelName = "Model.NotTrimmed";
            context.ModelMetadata = metadata;

            ModelBindingResult actual = binder.BindModelAsync(context).Result;
            ModelBindingResult expected = ModelBindingResult.NoResult;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void BindModelAsync_ReturnsNullResult(String value)
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(value));
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            ModelBindingResult expected = ModelBindingResult.Success("Model.Trimmed", null);
            ModelBindingResult actual = binder.BindModelAsync(context).Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void BindModelAsync_ReturnsEmptyResult(String value)
        {
            ModelMetadata metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(String)));
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(value));
            metadata.ConvertEmptyStringToNull.Returns(false);
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            ModelBindingResult expected = ModelBindingResult.Success("Model.Trimmed", "");
            ModelBindingResult actual = binder.BindModelAsync(context).Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        [Fact]
        public void BindModelAsync_ReturnsTrimmedResult()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(" Value "));
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            ModelBindingResult expected = ModelBindingResult.Success("Model.Trimmed", "Value");
            ModelBindingResult actual = binder.BindModelAsync(context).Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        #endregion
    }
}
