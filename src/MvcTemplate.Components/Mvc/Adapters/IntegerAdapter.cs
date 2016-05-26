using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MvcTemplate.Components.Mvc
{
    public class IntegerAdapter : ValidationAttributeAdapter<IntegerAttribute>
    {
        public IntegerAdapter(IntegerAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-integer", GetErrorMessage(context.ModelMetadata));
        }
    }
}
