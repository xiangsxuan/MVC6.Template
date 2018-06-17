using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System;

namespace MvcTemplate.Components.Mvc
{
    public class NumberValidator : IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-number"] = String.Format(Validations.Numeric, context.ModelMetadata.GetDisplayName());
        }
    }
}
