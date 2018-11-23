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
    public class StringLengthAdapterTests
    {
        private StringLengthAdapter adapter;
        private ModelValidationContextBase context;

        public StringLengthAdapterTests()
        {
            adapter = new StringLengthAdapter(new StringLengthAttribute(128));
            IModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelMetadata metadata = provider.GetMetadataForProperty(typeof(AllTypesView), "StringField");

            context = new ModelValidationContextBase(new ActionContext(), metadata, provider);
        }

        #region GetErrorMessage(ModelValidationContextBase context)

        [Fact]
        public void GetErrorMessage_StringLength()
        {
            adapter.Attribute.MinimumLength = 0;

            String expected = String.Format(Validations.StringLength, context.ModelMetadata.PropertyName, 128);
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(Validations.StringLength, adapter.Attribute.ErrorMessage);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetErrorMessage_StringLengthRange()
        {
            adapter.Attribute.MinimumLength = 4;

            String expected = String.Format(Validations.StringLengthRange, context.ModelMetadata.PropertyName, 128, 4);
            String actual = adapter.GetErrorMessage(context);

            Assert.Equal(Validations.StringLengthRange, adapter.Attribute.ErrorMessage);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
