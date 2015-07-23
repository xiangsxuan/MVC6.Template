using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class RequiredAdapter : RequiredAttributeAdapter
    {
        public RequiredAdapter(RequiredAttribute attribute)
            : base(attribute)
        {
            Attribute.ErrorMessage = Validations.FieldIsRequired;
        }
    }
}