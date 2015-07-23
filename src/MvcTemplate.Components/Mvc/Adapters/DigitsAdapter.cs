using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class DigitsAdapter : DataAnnotationsClientModelValidator<DigitsAttribute>
    {
        public DigitsAdapter(DigitsAttribute attribute)
            : base(attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            return new[] { new ModelClientValidationRule("digits", GetErrorMessage(context.ModelMetadata)) };
        }
    }
}
