using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class GreaterThanAdapterTests
    {
        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsGreaterValidationRule()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            GreaterThanAdapter adapter = new GreaterThanAdapter(new GreaterThanAttribute(128));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "GreaterThan");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, null);
            String expectedMessage = new GreaterThanAttribute(128).FormatErrorMessage("GreaterThan");
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();

            Assert.Equal(128M, actual.ValidationParameters["min"]);
            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("greater", actual.ValidationType);
            Assert.Single(actual.ValidationParameters);
        }

        #endregion
    }
}
