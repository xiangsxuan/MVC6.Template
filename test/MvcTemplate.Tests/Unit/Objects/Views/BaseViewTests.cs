using MvcTemplate.Objects;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class BaseViewTests
    {
        private BaseView view;

        public BaseViewTests()
        {
            view = new AccountView();
        }

        #region Property: Id

        [Fact]
        public void Id_ReturnsNotNull()
        {
            view.Id = null;

            Assert.NotNull(view.Id);
        }

        [Fact]
        public void Id_ReturnsUniqueValue()
        {
            String id = view.Id;
            view.Id = null;

            String expected = id;
            String actual = view.Id;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Id_ReturnsSameValue()
        {
            String expected = view.Id;
            String actual = view.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: CreationDate

        [Fact]
        public void CreationDate_ReturnsSameValue()
        {
            DateTime expected = view.CreationDate;
            DateTime actual = view.CreationDate;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
