using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ModelMessagesProviderTests
    {
        private ModelBindingMessageProvider provider;

        public ModelMessagesProviderTests()
        {
            provider = new ModelBindingMessageProvider();
            new ModelMessagesProvider(provider);
        }

        #region ModelMessagesProvider(ModelBindingMessageProvider provider)

        [Fact]
        public void ModelMessagesProvider_SetsAttemptedValueIsInvalidAccessor()
        {
            String actual = provider.AttemptedValueIsInvalidAccessor("Test", "Property");
            String expected = String.Format(Validations.InvalidField, "Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsUnknownValueIsInvalidAccessor()
        {
            String expected = String.Format(Validations.InvalidField, "Property");
            String actual = provider.UnknownValueIsInvalidAccessor("Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsMissingBindRequiredValueAccessor()
        {
            String actual = provider.MissingBindRequiredValueAccessor("Property");
            String expected = String.Format(Validations.Required, "Property");

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ModelMessagesProvider_SetsValueMustNotBeNullAccessor()
        {
            String expected = String.Format(Validations.Required, "Property");
            String actual = provider.ValueMustNotBeNullAccessor("Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_ValueIsInvalidAccessor()
        {
            String expected = String.Format(Validations.InvalidValue, "Value");
            String actual = provider.ValueIsInvalidAccessor("Value");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsValueMustBeANumberAccessor()
        {
            String expected = String.Format(Validations.Numeric, "Property");
            String actual = provider.ValueMustBeANumberAccessor("Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsMissingKeyOrValueAccessor()
        {
            String actual = provider.MissingKeyOrValueAccessor();
            String expected = Validations.RequiredValue;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
