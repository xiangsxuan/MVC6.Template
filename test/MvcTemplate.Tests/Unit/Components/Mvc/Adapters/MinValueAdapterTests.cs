using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MinValueAdapterTests
    {
        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsMinRangeValidationRule()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            MinValueAdapter adapter = new MinValueAdapter(new MinValueAttribute(128));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "MinValue");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, null);
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();
            String expectedMessage = new MinValueAttribute(128).FormatErrorMessage("MinValue");

            Assert.Equal(128M, actual.ValidationParameters["min"]);
            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("range", actual.ValidationType);
            Assert.Single(actual.ValidationParameters);
        }

        #endregion
    }
}
