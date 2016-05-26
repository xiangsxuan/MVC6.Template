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
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_EqualTo()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            EqualToAdapter adapter = new EqualToAdapter(new EqualToAttribute("StringLength"));
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "EqualTo");
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            adapter.AddValidation(context);

            Assert.Equal(String.Format(Validations.EqualTo, "EqualTo", ""), attributes["data-equalto"]);
            Assert.Equal("*.StringLength", attributes["data-equalto-other"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(3, attributes.Count);
        }

        #endregion
    }
}
