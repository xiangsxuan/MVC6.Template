using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class TrimmingModelBinderProviderTests
    {
        #region GetBinder(ModelBinderProviderContext context)

        [Fact]
        public void GetBinder_ForString()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelBinderProviderContext context = Substitute.For<ModelBinderProviderContext>();
            context.Metadata.Returns(provider.GetMetadataForProperty(typeof(ProviderModel), "Id"));

            Assert.IsType<TrimmingModelBinder>(new TrimmingModelBinderProvider().GetBinder(context));
        }

        [Fact]
        public void GetBinder_ForNotStringReturnsNull()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelBinderProviderContext context = Substitute.For<ModelBinderProviderContext>();
            context.Metadata.Returns(provider.GetMetadataForProperty(typeof(ProviderModel), "Date"));

            Assert.Null(new TrimmingModelBinderProvider().GetBinder(context));
        }

        #endregion
    }
}
