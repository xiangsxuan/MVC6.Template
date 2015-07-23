using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class GreaterThanAdapter : DataAnnotationsClientModelValidator<GreaterThanAttribute>
    {
        public GreaterThanAdapter(GreaterThanAttribute attribute)
            : base(attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule("greater", GetErrorMessage(context.ModelMetadata));
            validationRule.ValidationParameters.Add("min", Attribute.Minimum);

            return new[] { validationRule };
        }
    }
}
