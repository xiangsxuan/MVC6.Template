using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class MaxValueAdapter : DataAnnotationsClientModelValidator<MaxValueAttribute>
    {
        public MaxValueAdapter(MaxValueAttribute attribute)
            : base(attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule("range", GetErrorMessage(context.ModelMetadata));
            validationRule.ValidationParameters.Add("max", Attribute.Maximum);

            return new[] { validationRule };
        }
    }
}
