using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MinLengthAdapterTests
    {
        #region Constructor: MinLengthAdapter(MinLengthAttribute attribute)

        [Fact]
        public void MinLengthAdapter_SetsErrorMessage()
        {
            MinLengthAttribute attribute = new MinLengthAttribute(128);
            new MinLengthAdapter(attribute);

            String expected = Validations.FieldMustBeWithMinLengthOf;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
