using MvcTemplate.Components.Html;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Html
{
    public class JsTreeTests
    {
        #region Constructor: JsTree()

        [Fact]
        public void JsTree_CreatesEmpty()
        {
            JsTree actual = new JsTree();

            Assert.Empty(actual.Nodes);
            Assert.Empty(actual.SelectedIds);
        }

        #endregion
    }
}
