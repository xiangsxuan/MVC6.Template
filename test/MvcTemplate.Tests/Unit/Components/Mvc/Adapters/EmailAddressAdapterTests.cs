using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        #region EmailAddressAdapter(EmailAddressAttribute attribute)

        [Fact]
        public void EmailAddressAdapter_SetsErrorMessage()
        {
            String actual = attribute.ErrorMessage;
            String expected = Validations.Email;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_Email()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EmailAddress");
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            adapter.AddValidation(context);

            Assert.Equal(String.Format(Validations.Email, "EmailAddress"), attributes["data-email"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(2, attributes.Count);
        }

        #endregion
    }
}
