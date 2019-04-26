using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Renting.Resources;

namespace Renting.Components.Mvc
{
    public class DateValidator : IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-date"] = Validation.For("Date", context.ModelMetadata.GetDisplayName());
        }
    }
}
