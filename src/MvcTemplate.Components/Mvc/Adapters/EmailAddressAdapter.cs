using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class EmailAddressAdapter : DataAnnotationsClientModelValidator<EmailAddressAttribute>
    {
        public EmailAddressAdapter(EmailAddressAttribute attribute)
            : base(attribute)
        {
            Attribute.ErrorMessage = Validations.FieldIsNotValidEmail;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            return new[] { new ModelClientValidationRule("email", GetErrorMessage(context.ModelMetadata)) };
        }
    }
}
