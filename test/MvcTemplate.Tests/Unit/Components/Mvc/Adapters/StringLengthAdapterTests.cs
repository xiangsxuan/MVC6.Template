using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class StringLengthAdapterTests
    {
        #region Constructor: StringLengthAdapter(StringLengthAttribute attribute)

        [Fact]
        public void StringLengthAdapter_SetsExceededErrorMessage()
        {
            StringLengthAttribute attribute = new StringLengthAttribute(128);
            new StringLengthAdapter(attribute);

            String expected = Validations.StringLength;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StringLengthAdapter_SetsRangeErrorMessage()
        {
            StringLengthAttribute attribute = new StringLengthAttribute(128) { MinimumLength = 4 };
            new StringLengthAdapter(attribute);

            String expected = Validations.StringLengthRange;
            String actual = attribute.ErrorMessage;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
