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
    public class IntegerAdapterTests
    {
        #region AddValidation(ClientModelValidationContext context)

        [Fact]
        public void AddValidation_Integer()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            IntegerAdapter adapter = new IntegerAdapter(new IntegerAttribute());
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AdaptersModel), "Integer");
            ClientModelValidationContext context = new ClientModelValidationContext(new ActionContext(), metadata, provider, attributes);

            adapter.AddValidation(context);

            Assert.Equal(String.Format(Validations.Integer, "Integer"), attributes["data-integer"]);
            Assert.Equal("true", attributes["data-val"]);
            Assert.Equal(2, attributes.Count);
        }

        #endregion
    }
}
