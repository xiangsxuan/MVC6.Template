using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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
            context = new DefaultModelBindingContext();
            context.ModelState = new ModelStateDictionary();
            context.ValueProvider = Substitute.For<IValueProvider>();
        }

        #region BindModelAsync(ModelBindingContext context)

        [Fact]
        public void BindModelAsync_NoValue()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(String));
            context.ValueProvider.GetValue(context.ModelName).Returns(ValueProviderResult.None);
            context.ModelMetadata = metadata;

            binder.BindModelAsync(context).Wait();

            Assert.Null(context.Result);
        }

        [Fact]
        public void BindModelAsync_NotTrimmed()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "NotTrimmed");
            context.ValueProvider.GetValue("Model.NotTrimmed").Returns(new ValueProviderResult(" Value "));
            context.ModelName = "Model.NotTrimmed";
            context.ModelMetadata = metadata;

            binder.BindModelAsync(context).Wait();

            ModelBindingResult expected = ModelBindingResult.Success("Model.NotTrimmed", " Value ");
            ModelBindingResult actual = context.Result.Value;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void BindModelAsync_Null(String value)
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(value));
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            binder.BindModelAsync(context).Wait();

            ModelBindingResult expected = ModelBindingResult.Success("Model.Trimmed", null);
            ModelBindingResult actual = context.Result.Value;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void BindModelAsync_Empty(String value)
        {
            ModelMetadata metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(String)));
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(value));
            metadata.ConvertEmptyStringToNull.Returns(false);
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            binder.BindModelAsync(context).Wait();

            ModelBindingResult expected = ModelBindingResult.Success("Model.Trimmed", "");
            ModelBindingResult actual = context.Result.Value;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        [Fact]
        public void BindModelAsync_Trimmed()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(" Value "));
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            binder.BindModelAsync(context).Wait();

            ModelBindingResult expected = ModelBindingResult.Success("Model.Trimmed", "Value");
            ModelBindingResult actual = context.Result.Value;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.Key, actual.Key);
        }

        #endregion
    }
}
