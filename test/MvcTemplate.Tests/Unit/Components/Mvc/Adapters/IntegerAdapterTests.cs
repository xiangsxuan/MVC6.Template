using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class IntegerAdapterTests
    {
        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsMinRangeValidationRule()
        {
            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Integer");
            String errorMessage = new IntegerAttribute().FormatErrorMessage("Integer");
            IntegerAdapter adapter = new IntegerAdapter(new IntegerAttribute());

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, Substitute.For<IServiceProvider>());
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();

            Assert.Equal(errorMessage, actual.ErrorMessage);
            Assert.Equal("integer", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
