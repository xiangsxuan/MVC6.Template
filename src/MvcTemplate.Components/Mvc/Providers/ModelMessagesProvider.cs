using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Resources.Form;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class ModelMessagesProvider
    {
        public ModelMessagesProvider(ModelBindingMessageProvider provider)
        {
            provider.AttemptedValueIsInvalidAccessor = (value, field) => String.Format(Validations.InvalidField, field);
            provider.UnknownValueIsInvalidAccessor = (field) => String.Format(Validations.InvalidField, field);
            provider.MissingBindRequiredValueAccessor = (field) => String.Format(Validations.Required, field);
            provider.ValueMustNotBeNullAccessor = (field) => String.Format(Validations.Required, field);
            provider.ValueIsInvalidAccessor = (value) => String.Format(Validations.InvalidValue, value);
            provider.ValueMustBeANumberAccessor = (field) => String.Format(Validations.Numeric, field);
            provider.MissingKeyOrValueAccessor = () => Validations.RequiredValue;
        }
    }
}
