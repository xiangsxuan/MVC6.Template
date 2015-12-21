using MvcTemplate.Components.Html;
using MvcTemplate.Objects;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class RoleViewTests
    {
        #region Constructor: RoleView()

        [Fact]
        public void RoleView_CreatesEmpty()
        {
            JsTree actual = new RoleView().Permissions;

            Assert.Empty(actual.SelectedIds);
            Assert.Empty(actual.Nodes);
        }

        #endregion
    }
}
