using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class EqualToAdapterTests
    {
        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_SetsOtherPropertyDisplayName()
        {
            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EqualTo");
            EqualToAttribute attribute = new EqualToAttribute("EqualTo");
            attribute.OtherPropertyDisplayName = null;

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, null);
            new EqualToAdapter(attribute).GetClientValidationRules(context);

            String expected = ResourceProvider.GetPropertyTitle(typeof(AdaptersModel), "EqualTo");
            String actual = attribute.OtherPropertyDisplayName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetClientValidationRules_ReturnsEqualToValidationRule()
        {
            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EqualTo");
            EqualToAdapter adapter = new EqualToAdapter(new EqualToAttribute("StringLength"));

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, null);
            String expectedMessage = new EqualToAttribute("StringLength").FormatErrorMessage("EqualTo");
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();

            Assert.Equal("*.StringLength", actual.ValidationParameters["other"]);
            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("equalto", actual.ValidationType);
            Assert.Single(actual.ValidationParameters);
        }

        #endregion
    }
}
