using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class EqualToAdapter : AttributeAdapterBase<EqualToAttribute>
    {
        public EqualToAdapter(EqualToAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-equalto"] = GetErrorMessage(context);
            context.Attributes["data-val-equalto-other"] = "*." + Attribute.OtherPropertyName;

            Attribute.OtherPropertyDisplayName = ResourceProvider.GetPropertyTitle(context.ModelMetadata.ContainerType, Attribute.OtherPropertyName);
        }
        public override String GetErrorMessage(ModelValidationContextBase context)
        {
            return GetErrorMessage(context.ModelMetadata);
        }
    }
}
