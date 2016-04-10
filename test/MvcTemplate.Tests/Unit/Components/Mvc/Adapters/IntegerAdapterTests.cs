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
    public class IntegerAdapterTests
    {
        #region GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsIntegerValidationRule()
        {
            IServiceProvider services = Substitute.For<IServiceProvider>();
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            IntegerAdapter adapter = new IntegerAdapter(new IntegerAttribute());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Integer");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, services);
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();
            String expectedMessage = new IntegerAttribute().FormatErrorMessage("Integer");

            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("integer", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
