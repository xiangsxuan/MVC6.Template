using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class StringLengthAdapter : StringLengthAttributeAdapter
    {
        public StringLengthAdapter(StringLengthAttribute attribute)
            : base(attribute, null)
        {
            if (Attribute.MinimumLength == 0)
                Attribute.ErrorMessage = Validations.StringLength;
            else
                Attribute.ErrorMessage = Validations.StringLengthRange;
        }
    }
}