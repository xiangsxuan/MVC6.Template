using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MvcTemplate.Resources.Form;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class ModelMessagesProvider
    {
        public ModelMessagesProvider(ModelBindingMessageProvider messages)
        {
            messages.AttemptedValueIsInvalidAccessor = (value, field) => String.Format(Validations.InvalidField, field);
            messages.UnknownValueIsInvalidAccessor = (field) => String.Format(Validations.InvalidField, field);
            messages.MissingBindRequiredValueAccessor = (field) => String.Format(Validations.Required, field);
            messages.ValueMustNotBeNullAccessor = (field) => String.Format(Validations.Required, field);
            messages.ValueIsInvalidAccessor = (value) => String.Format(Validations.InvalidValue, value);
            messages.ValueMustBeANumberAccessor = (field) => String.Format(Validations.Numeric, field);
            messages.MissingKeyOrValueAccessor = () => Validations.RequiredValue;
        }
    }
}
