using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class RequiredAdapter : RequiredAttributeAdapter
    {
        public RequiredAdapter(RequiredAttribute attribute)
            : base(attribute, null)
        {
            Attribute.ErrorMessage = Validations.Required;
        }
    }
}