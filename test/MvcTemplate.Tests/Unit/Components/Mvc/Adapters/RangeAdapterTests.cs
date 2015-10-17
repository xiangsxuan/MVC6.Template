using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class RangeAdapterTests
    {
        #region Constructor: RangeAdapter(RangeAttribute attribute)

        [Fact]
        public void RangeAdapter_SetsErrorMessage()
        {
            RangeAttribute attribute = new RangeAttribute(0, 128);
            new RangeAdapter(attribute);

            String expected = Validations.FieldMustBeInRange;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
