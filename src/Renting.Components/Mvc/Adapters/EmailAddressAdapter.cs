﻿using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Renting.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Renting.Components.Mvc
{
    public class EmailAddressAdapter : AttributeAdapterBase<EmailAddressAttribute>
    {
        public EmailAddressAdapter(EmailAddressAttribute attribute)
            : base(attribute, null)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-email"] = GetErrorMessage(context);
        }
        public override String GetErrorMessage(ModelValidationContextBase context)
        {
            Attribute.ErrorMessage = Validation.For("Email");

            return GetErrorMessage(context.ModelMetadata);
        }
    }
}
