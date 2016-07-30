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
    public class EqualToAdapterTests
    {
        private EqualToAdapter adapter;
        private ClientModelValidationContext context;
        private Dictionary<String, String> attributes;

        public EqualToAdapterTests()
        {
            attributes = new Dictionary<String, String>();
            adapter = new EqualToAdapter(new EqualToAttribute("StringLength"));
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EqualTo");
            context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);
        }

        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_EqualTo()
        {
            adapter.AddValidation(context);

            Assert.Equal(3, attributes.Count);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal("*.StringLength", attributes["data-val-equalto-other"]);
            Assert.Equal(String.Format(Validations.EqualTo, "EqualTo", "StringLength"), attributes["data-val-equalto"]);
        }

        #endregion

        #region GetErrorMessage(ModelValidationContextBase context)

        [Fact]
        public void GetErrorMessage_EqualTo()
        {
            String expected = String.Format(Validations.EqualTo, "EqualTo", "");
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
