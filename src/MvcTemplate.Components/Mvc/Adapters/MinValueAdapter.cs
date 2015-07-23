using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class MinValueAdapter : DataAnnotationsClientModelValidator<MinValueAttribute>
    {
        public MinValueAdapter(MinValueAttribute attribute)
            : base(attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule("range", GetErrorMessage(context.ModelMetadata));
            validationRule.ValidationParameters.Add("min", Attribute.Minimum);

            return new[] { validationRule };
        }
    }
}
