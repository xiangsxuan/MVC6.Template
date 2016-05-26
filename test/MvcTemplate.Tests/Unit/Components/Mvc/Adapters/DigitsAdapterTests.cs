using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DigitsAdapterTests
    {
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_Digits()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Digits");
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            new DigitsAdapter(new DigitsAttribute()).AddValidation(context);

            Assert.Equal(String.Format(Validations.Digits, "Digits"), attributes["data-digits"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(2, attributes.Count);
        }

        #endregion
    }
}
