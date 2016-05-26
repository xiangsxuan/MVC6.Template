using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources;

namespace MvcTemplate.Components.Mvc
{
    public class EqualToAdapter : ValidationAttributeAdapter<EqualToAttribute>
    {
        public EqualToAdapter(EqualToAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-equalto"] = GetErrorMessage(context.ModelMetadata);
            context.Attributes["data-equalto-other"] = "*." + Attribute.OtherPropertyName;

            Attribute.OtherPropertyDisplayName = ResourceProvider.GetPropertyTitle(context.ModelMetadata.ContainerType, Attribute.OtherPropertyName);
        }
    }
}
