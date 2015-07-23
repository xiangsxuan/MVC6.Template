using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class IntegerAdapter : DataAnnotationsClientModelValidator<IntegerAttribute>
    {
        public IntegerAdapter(IntegerAttribute attribute)
            : base(attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            return new[] { new ModelClientValidationRule("integer", GetErrorMessage(context.ModelMetadata)) };
        }
    }
}
