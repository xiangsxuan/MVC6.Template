using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class EmailAddressAdapterTests
    {
        private EmailAddressAttribute attribute;
        private EmailAddressAdapter adapter;

        public EmailAddressAdapterTests()
        {
            attribute = new EmailAddressAttribute();
            adapter = new EmailAddressAdapter(attribute);
        }

        #region Constructor: EmailAddressAdapter(EmailAddressAttribute attribute)

        [Fact]
        public void EmailAddressAdapter_SetsErrorMessage()
        {
            String actual = attribute.ErrorMessage;
            String expected = Validations.Email;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsEmailValidationRule()
        {
            IServiceProvider services = Substitute.For<IServiceProvider>();
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EmailAddress");

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, services);
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();
            String expectedMessage = attribute.FormatErrorMessage(metadata.GetDisplayName());

            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("email", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
