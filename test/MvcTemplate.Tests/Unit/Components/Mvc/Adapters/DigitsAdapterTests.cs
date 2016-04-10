using Microsoft.AspNet.Mvc.ModelBinding;
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
        #region GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsDigitsValidationRule()
        {
            IServiceProvider services = Substitute.For<IServiceProvider>();
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Digits");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, services);

            ModelClientValidationRule actual = new DigitsAdapter(new DigitsAttribute()).GetClientValidationRules(context).Single();
            String expectedMessage = new DigitsAttribute().FormatErrorMessage(metadata.GetDisplayName());

            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("digits", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
