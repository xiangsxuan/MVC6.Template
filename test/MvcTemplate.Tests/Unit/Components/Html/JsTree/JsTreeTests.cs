using MvcTemplate.Components.Html;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Html
{
    public class JsTreeTests
    {
        #region Constructor: JsTree()

        [Fact]
        public void JsTree_CreatesEmptyTree()
        {
            Assert.Empty(new JsTree().Nodes);
        }

        [Fact]
        public void JsTree_CreatesUnselectedTree()
        {
            Assert.Empty(new JsTree().SelectedIds);
        }

        #endregion
    }
}
