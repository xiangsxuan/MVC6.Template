using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Globalization;

namespace MvcTemplate.Components.Mvc
{
    public class GreaterThanAdapter : ValidationAttributeAdapter<GreaterThanAttribute>
    {
        public GreaterThanAdapter(GreaterThanAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-greater"] = GetErrorMessage(context.ModelMetadata);
            context.Attributes["data-greater-min"] = Attribute.Minimum.ToString(CultureInfo.InvariantCulture);
        }
    }
}
