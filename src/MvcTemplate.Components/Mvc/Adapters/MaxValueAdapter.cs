using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Globalization;

namespace MvcTemplate.Components.Mvc
{
    public class MaxValueAdapter : ValidationAttributeAdapter<MaxValueAttribute>
    {
        public MaxValueAdapter(MaxValueAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-range"] = GetErrorMessage(context.ModelMetadata);
            context.Attributes["data-range-max"] = Attribute.Maximum.ToString(CultureInfo.InvariantCulture);
        }
    }
}
