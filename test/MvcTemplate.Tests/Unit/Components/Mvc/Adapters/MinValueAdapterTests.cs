using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests;
using System;
using System.Collections.Generic;
using Xunit;

namespace MvcTemplate.Components.Mvc.Tests
{
    public class MinValueAdapterTests
    {
        private MinValueAdapter adapter;
        private ClientModelValidationContext context;
        private Dictionary<String, String> attributes;

        public MinValueAdapterTests()
        {
            attributes = new Dictionary<String, String>();
            adapter = new MinValueAdapter(new MinValueAttribute(128));
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "Int32Field");
            context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
        }

        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_MinValue()
        {
            adapter.AddValidation(context);

            Assert.Equal(3, attributes.Count);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal("128", attributes["data-val-range-min"]);
            Assert.Equal(String.Format(Validations.MinValue, context.ModelMetadata.PropertyName, 128), attributes["data-val-range"]);
        }

        #endregion

        #region GetErrorMessage(ModelValidationContextBase context)

        [Fact]
        public void GetErrorMessage_MinValue()
        {
            String expected = String.Format(Validations.MinValue, context.ModelMetadata.PropertyName, 128);
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
