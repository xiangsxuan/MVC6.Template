using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Form;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MaxValueAttributeTests
    {
        private MaxValueAttribute attribute;

        public MaxValueAttributeTests()
        {
            attribute = new MaxValueAttribute(12.56);
        }

        #region Constructor: MaxValueAttribute(Int32 maximum)

        [Fact]
        public void MaxValueAttribute_SetsMaximumFromInteger()
        {
            Decimal actual = new MaxValueAttribute(10).Maximum;
            Decimal expected = 10M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: MaxValueAttribute(Double maximum)

        [Fact]
        public void MaxValueAttribute_SetsMaximumFromDouble()
        {
            Decimal actual = new MaxValueAttribute(12.56).Maximum;
            Decimal expected = 12.56M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Fact]
        public void FormatErrorMessage_FormatsErrorMessageForInteger()
        {
            attribute = new MaxValueAttribute(10);

            String expected = String.Format(Validations.FieldMustBeLessOrEqualTo, "Sum", attribute.Maximum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatErrorMessage_FormatsErrorMessageForDouble()
        {
            attribute = new MaxValueAttribute(13.44);

            String expected = String.Format(Validations.FieldMustBeLessOrEqualTo, "Sum", attribute.Maximum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: IsValid(Object value)

        [Fact]
        public void IsValid_NullValueIsValid()
        {
            Assert.True(attribute.IsValid(null));
        }

        [Theory]
        [InlineData(12.56)]
        [InlineData("12.559")]
        public void IsValid_LowerOrEqualValueIsValid(Object value)
        {
            Assert.True(attribute.IsValid(value));
        }

        [Fact]
        public void IsValid_GreaterValueIsNotValid()
        {
            Assert.False(attribute.IsValid(12.5601));
        }

        [Fact]
        public void IsValid_NotDecimalValueIsNotValid()
        {
            Assert.False(attribute.IsValid("12.56M"));
        }

        #endregion
    }
}
