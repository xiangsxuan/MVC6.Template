using MvcTemplate.Components.Html;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Html
{
    public class JsTreeNodeTests
    {
        #region Constructor: JsTreeNode(String title)

        [Fact]
        public void JsTreeNode_SetsTitle()
        {
            JsTreeNode actual = new JsTreeNode("Title");

            Assert.Equal("Title", actual.Title);
            Assert.Empty(actual.Nodes);
            Assert.Null(actual.Id);
        }

        #endregion

        #region Constructor: JsTreeNode(Int32? id, String title)

        [Fact]
        public void JsTreeNode_SetsIdAndTitle()
        {
            JsTreeNode actual = new JsTreeNode(1, "Title");

            Assert.Equal("Title", actual.Title);
            Assert.Equal(1, actual.Id);
            Assert.Empty(actual.Nodes);
        }

        #endregion
    }
}
