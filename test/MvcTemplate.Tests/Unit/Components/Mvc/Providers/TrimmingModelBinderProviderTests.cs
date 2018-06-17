using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class TrimmingModelBinderProviderTests
    {
        #region GetBinder(ModelBinderProviderContext context)

        [Fact]
        public void GetBinder_ForString()
        {
            ModelBinderProviderContext context = Substitute.For<ModelBinderProviderContext>();
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();

            context.Metadata.Returns(provider.GetMetadataForType(typeof(String)));

            Assert.IsType<TrimmingModelBinder>(new TrimmingModelBinderProvider().GetBinder(context));
        }

        [Fact]
        public void GetBinder_ForNotStringReturnsNull()
        {
            ModelBinderProviderContext context = Substitute.For<ModelBinderProviderContext>();
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();

            context.Metadata.Returns(provider.GetMetadataForType(typeof(DateTime)));

            Assert.Null(new TrimmingModelBinderProvider().GetBinder(context));
        }

        #endregion
    }
}
