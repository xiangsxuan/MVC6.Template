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

        #region Property: Id

        [Fact]
        public void Id_ReturnsNotNull()
        {
            model.Id = null;

            Assert.NotNull(model.Id);
        }

        [Fact]
        public void Id_ReturnsUniqueValue()
        {
            String id = model.Id;
            model.Id = null;

            String expected = id;
            String actual = model.Id;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Id_ReturnsSameValue()
        {
            String expected = model.Id;
            String actual = model.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: CreationDate

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
