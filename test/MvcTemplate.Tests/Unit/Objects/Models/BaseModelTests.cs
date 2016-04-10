using MvcTemplate.Objects;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class BaseModelTests
    {
        private BaseModel model;

        public BaseModelTests()
        {
            model = new Account();
        }

        #region CreationDate

        [Fact]
        public void CreationDate_ReturnsSameValue()
        {
            DateTime expected = model.CreationDate;
            DateTime actual = model.CreationDate;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
