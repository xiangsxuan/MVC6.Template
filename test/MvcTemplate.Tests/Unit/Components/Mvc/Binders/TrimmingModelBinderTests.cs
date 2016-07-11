using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Threading.Tasks;
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
        public async Task BindModelAsync_NoValue()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(String));
            context.ValueProvider.GetValue(context.ModelName).Returns(ValueProviderResult.None);
            context.ModelMetadata = metadata;

            await binder.BindModelAsync(context);

            ModelBindingResult expected = new ModelBindingResult();
            ModelBindingResult actual = context.Result;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task BindModelAsync_NotTrimmed()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "NotTrimmed");
            context.ValueProvider.GetValue("Model.NotTrimmed").Returns(new ValueProviderResult(" Value "));
            context.ModelName = "Model.NotTrimmed";
            context.ModelMetadata = metadata;

            await binder.BindModelAsync(context);

            ModelBindingResult expected = ModelBindingResult.Success(" Value ");
            ModelBindingResult actual = context.Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public async Task BindModelAsync_Null(String value)
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(value));
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            await binder.BindModelAsync(context);

            ModelBindingResult expected = ModelBindingResult.Success(null);
            ModelBindingResult actual = context.Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public async Task BindModelAsync_Empty(String value)
        {
            ModelMetadata metadata = Substitute.For<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(String)));
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(value));
            metadata.ConvertEmptyStringToNull.Returns(false);
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            await binder.BindModelAsync(context);

            ModelBindingResult expected = ModelBindingResult.Success("");
            ModelBindingResult actual = context.Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
        }

        [Fact]
        public async Task BindModelAsync_Trimmed()
        {
            ModelMetadata metadata = new EmptyModelMetadataProvider().GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            context.ValueProvider.GetValue("Model.Trimmed").Returns(new ValueProviderResult(" Value "));
            context.ModelName = "Model.Trimmed";
            context.ModelMetadata = metadata;

            await binder.BindModelAsync(context);

            ModelBindingResult expected = ModelBindingResult.Success("Value");
            ModelBindingResult actual = context.Result;

            Assert.Equal(expected.IsModelSet, actual.IsModelSet);
            Assert.Equal(expected.Model, actual.Model);
        }

        #endregion
    }
}
