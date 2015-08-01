using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
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
        private ModelBindingContext bindingContext;
        private TrimmingModelBinder binder;

        public TrimmingModelBinderTests()
        {
            bindingContext = new ModelBindingContext { ModelName = "Trimmed" };
            bindingContext.ValueProvider = Substitute.For<IValueProvider>();
            binder = new TrimmingModelBinder();
        }

        #region Method: BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)

        [Fact]
        public void BindModel_ReturnNullThenThereIsNoValueProvider()
        {
            Task<ValueProviderResult> result = Task.FromResult<ValueProviderResult>(null);
            bindingContext.ValueProvider.GetValueAsync(bindingContext.ModelName).Returns(result);

            Assert.Null(binder.BindModelAsync(bindingContext).Result.Model);
        }

        [Fact]
        public void BindModel_OnNullValueReturnsNull()
        {
            Task<ValueProviderResult> result = Task.FromResult(new ValueProviderResult(null));
            bindingContext.ValueProvider.GetValueAsync(bindingContext.ModelName).Returns(result);

            Assert.Null(binder.BindModelAsync(bindingContext).Result.Model);
        }

        [Fact]
        public void BindModel_DoesNotTrimPropertyWithNotTrimmedAttribute()
        {
            Task<ValueProviderResult> result = Task.FromResult(new ValueProviderResult("  Trimmed text  ", "  Trimmed text  ", null));
            bindingContext.ValueProvider.GetValueAsync("NotTrimmed").Returns(result);
            bindingContext.ModelName = "NotTrimmed";

            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(BindersModel), "NotTrimmed");
            bindingContext.ModelMetadata = metadata;

            Object actual = binder.BindModelAsync(bindingContext).Result.Model;
            Object expected = "  Trimmed text  ";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BindModel_TrimsBindedModelsProperty()
        {
            Task<ValueProviderResult> result = Task.FromResult(new ValueProviderResult("  Trimmed text  ", "  Trimmed text  ", null));
            bindingContext.ValueProvider.GetValueAsync("Trimmed").Returns(result);
            bindingContext.ModelName = "Trimmed";

            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(BindersModel), "Trimmed");
            bindingContext.ModelMetadata = metadata;

            Object actual = binder.BindModelAsync(bindingContext).Result.Model;
            Object expected = "Trimmed text";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
