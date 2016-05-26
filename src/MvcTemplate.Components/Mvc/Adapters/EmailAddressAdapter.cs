using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class EmailAddressAdapter : ValidationAttributeAdapter<EmailAddressAttribute>
    {
        public EmailAddressAdapter(EmailAddressAttribute attribute)
            : base(attribute, null)
        {
            Attribute.ErrorMessage = Validations.Email;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-email", GetErrorMessage(context.ModelMetadata));
        }
    }
}
