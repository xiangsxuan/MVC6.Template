using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MaxValueAdapterTests
    {
        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsMaxRangeValidationRule()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            MaxValueAdapter adapter = new MaxValueAdapter(new MaxValueAttribute(128));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "MaxValue");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, null);
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();
            String expectedMessage = new MaxValueAttribute(128).FormatErrorMessage("MaxValue");

            Assert.Equal(128M, actual.ValidationParameters["max"]);
            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("range", actual.ValidationType);
            Assert.Single(actual.ValidationParameters);
        }

        #endregion
    }
}
