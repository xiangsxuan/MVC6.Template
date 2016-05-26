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
    public class MaxValueAdapterTests
    {
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_MaxValue()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            MaxValueAdapter adapter = new MaxValueAdapter(new MaxValueAttribute(128));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "MaxValue");
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            adapter.AddValidation(context);

            Assert.Equal(String.Format(Validations.MaxValue, "MaxValue", 128), attributes["data-range"]);
            Assert.Equal("128", attributes["data-range-max"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(3, attributes.Count);
        }

        #endregion
    }
}
