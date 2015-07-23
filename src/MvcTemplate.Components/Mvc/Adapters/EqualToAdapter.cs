using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public class EqualToAdapter : DataAnnotationsClientModelValidator<EqualToAttribute>
    {
        public EqualToAdapter(EqualToAttribute attribute)
            : base(attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            Attribute.OtherPropertyDisplayName = ResourceProvider.GetPropertyTitle(context.ModelMetadata.ContainerType, Attribute.OtherPropertyName);
            ModelClientValidationRule validationRule = new ModelClientValidationRule("equalto", GetErrorMessage(context.ModelMetadata));
            validationRule.ValidationParameters.Add("other", "*." + Attribute.OtherPropertyName);

            return new[] { validationRule };
        }
    }
}
