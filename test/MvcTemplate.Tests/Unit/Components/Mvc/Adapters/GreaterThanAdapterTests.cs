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
    public class GreaterThanAdapterTests
    {
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_GreaterThan()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            GreaterThanAdapter adapter = new GreaterThanAdapter(new GreaterThanAttribute(128));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "GreaterThan");

            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            adapter.AddValidation(context);

            Assert.Equal(String.Format(Validations.GreaterThan, "GreaterThan", 128), attributes["data-greater"]);
            Assert.Equal("128", attributes["data-greater-min"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(3, attributes.Count);
        }

        #endregion
    }
}
