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
    public class DigitsAdapterTests
    {
        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsMinRangeValidationRule()
        {
            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Digits");
            String errorMessage = new DigitsAttribute().FormatErrorMessage(metadata.GetDisplayName());

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, Substitute.For<IServiceProvider>());
            ModelClientValidationRule actual = new DigitsAdapter(new DigitsAttribute()).GetClientValidationRules(context).Single();

            Assert.Equal(errorMessage, actual.ErrorMessage);
            Assert.Equal("digits", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
