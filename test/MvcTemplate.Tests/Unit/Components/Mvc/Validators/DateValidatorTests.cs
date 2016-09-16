using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class DateValidatorTests
    {
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_Date()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForType(typeof(DateTime));
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            new DateValidator().AddValidation(context);

            Assert.Equal(2, attributes.Count);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(String.Format(Validations.Date, "DateTime"), attributes["data-val-date"]);
        }

        #endregion
    }
}
