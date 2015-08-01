using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
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
            String expected = Validations.FieldIsNotValidEmail;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetClientValidationRules(ClientModelValidationContext context)

        [Fact]
        public void GetClientValidationRules_ReturnsEmailValidationRule()
        {
            IModelMetadataProvider provider = new DefaultModelMetadataProvider(Substitute.For<ICompositeMetadataDetailsProvider>());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EmailAddress");
            String errorMessage = attribute.FormatErrorMessage(metadata.GetDisplayName());

            ClientModelValidationContext context = new ClientModelValidationContext(metadata, provider, Substitute.For<IServiceProvider>());
            ModelClientValidationRule actual = adapter.GetClientValidationRules(context).Single();

            Assert.Equal(errorMessage, actual.ErrorMessage);
            Assert.Equal("email", actual.ValidationType);
            Assert.Empty(actual.ValidationParameters);
        }

        #endregion
    }
}
