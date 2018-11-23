using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        public Decimal Minimum { get; }

        public GreaterThanAttribute(Double minimum)
            : base(() => Validations.GreaterThan)
        {
            Minimum = Convert.ToDecimal(minimum);
        }

        public override String FormatErrorMessage(String name)
        {
            return String.Format(ErrorMessageString, name, Minimum);
        }
        public override Boolean IsValid(Object value)
        {
            try
            {
                return value == null || Convert.ToDecimal(value) > Minimum;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
