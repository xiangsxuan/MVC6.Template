using MvcTemplate.Components.Mvc;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class NotTrimmedAttributeTests
    {
        #region NotTrimmedAttribute()

        [Fact]
        public void NotTrimmedAttribute_SetsBinderType()
        {
            Type actual = new NotTrimmedAttribute().BinderType;
            Type expected = typeof(NotTrimmedAttribute);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
