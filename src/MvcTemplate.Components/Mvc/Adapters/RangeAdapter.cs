using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class RangeAdapter : RangeAttributeAdapter
    {
        public RangeAdapter(RangeAttribute attribute)
            : base(attribute)
        {
            Attribute.ErrorMessage = Validations.FieldMustBeInRange;
        }
    }
}
