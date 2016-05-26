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
    public class MinValueAdapterTests
    {
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_MinValue()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            MinValueAdapter adapter = new MinValueAdapter(new MinValueAttribute(128));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "MinValue");
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            adapter.AddValidation(context);

            Assert.Equal(String.Format(Validations.MinValue, "MinValue", 128), attributes["data-range"]);
            Assert.Equal("128", attributes["data-range-min"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(3, attributes.Count);
        }

        #endregion
    }
}
