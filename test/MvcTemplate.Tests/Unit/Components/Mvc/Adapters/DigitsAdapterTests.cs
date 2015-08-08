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
        public void GetClientValidationRules_ReturnsDigitsValidationRule()
        {
            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Digits");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, null);

            ModelClientValidationRule actual = new DigitsAdapter(new DigitsAttribute()).GetClientValidationRules(context).Single();
            String expectedMessage = new DigitsAttribute().FormatErrorMessage(metadata.GetDisplayName());

            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("digits", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
