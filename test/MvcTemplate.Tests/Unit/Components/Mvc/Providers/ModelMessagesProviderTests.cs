using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ModelMessagesProviderTests
    {
        private DefaultModelBindingMessageProvider messages;

        public ModelMessagesProviderTests()
        {
            messages = new DefaultModelBindingMessageProvider();
            new ModelMessagesProvider(messages);
        }

        #region ModelMessagesProvider(ModelBindingMessageProvider messages)

        [Fact]
        public void ModelMessagesProvider_SetsAttemptedValueIsInvalidAccessor()
        {
            String actual = messages.AttemptedValueIsInvalidAccessor("Test", "Property");
            String expected = String.Format(Validations.InvalidField, "Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsUnknownValueIsInvalidAccessor()
        {
            String expected = String.Format(Validations.InvalidField, "Property");
            String actual = messages.UnknownValueIsInvalidAccessor("Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsMissingBindRequiredValueAccessor()
        {
            String actual = messages.MissingBindRequiredValueAccessor("Property");
            String expected = String.Format(Validations.Required, "Property");

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ModelMessagesProvider_SetsValueMustNotBeNullAccessor()
        {
            String expected = String.Format(Validations.Required, "Property");
            String actual = messages.ValueMustNotBeNullAccessor("Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_ValueIsInvalidAccessor()
        {
            String expected = String.Format(Validations.InvalidValue, "Value");
            String actual = messages.ValueIsInvalidAccessor("Value");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsValueMustBeANumberAccessor()
        {
            String expected = String.Format(Validations.Numeric, "Property");
            String actual = messages.ValueMustBeANumberAccessor("Property");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModelMessagesProvider_SetsMissingKeyOrValueAccessor()
        {
            String actual = messages.MissingKeyOrValueAccessor();
            String expected = Validations.RequiredValue;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
