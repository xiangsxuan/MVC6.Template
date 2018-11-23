using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using MvcTemplate.Tests;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MvcTemplate.Components.Mvc.Tests
{
    public class RequiredAdapterTests
    {
        #region GetErrorMessage(ModelValidationContextBase context)

        [Fact]
        public void GetErrorMessage_Required()
        {
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            RequiredAdapter adapter = new RequiredAdapter(new RequiredAttribute());
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "StringField");
            ModelValidationContextBase context = new ModelValidationContextBase(new ActionContext(), metadata, provider);

            String expected = String.Format(Validations.Required, context.ModelMetadata.PropertyName);
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(Validations.Required, adapter.Attribute.ErrorMessage);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
