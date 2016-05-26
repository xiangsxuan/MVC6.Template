using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MvcTemplate.Components.Mvc
{
    public class DigitsAdapter : ValidationAttributeAdapter<DigitsAttribute>
    {
        public DigitsAdapter(DigitsAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-digits", GetErrorMessage(context.ModelMetadata));
        }
    }
}
