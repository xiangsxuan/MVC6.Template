using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcTemplate.Components.Extensions;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Unit.Components.Extensions;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class JsTreeTagHelperTests
    {
        private JsTreeTagHelper helper;
        private TagHelperOutput output;

        public JsTreeTagHelperTests()
        {
            JsTree tree = new JsTree();
            tree.SelectedIds.Add(4567);
            tree.SelectedIds.Add(12345);
            tree.Nodes.Add(new JsTreeNode("Test"));
            tree.Nodes[0].Nodes.Add(new JsTreeNode(12345, "Test1"));
            tree.Nodes[0].Nodes.Add(new JsTreeNode(23456, "Test2"));

            EmptyModelMetadataProvider provider = new EmptyModelMetadataProvider();
            ModelExplorer explorer = new ModelExplorer(provider, provider.GetMetadataForProperty(typeof(JsTreeView), "JsTree"), tree);

            helper = new JsTreeTagHelper();
            helper.For = new ModelExpression("JsTree", explorer);
            output = new TagHelperOutput("div", new TagHelperAttributeList(), (useCachedResult, encoder) => null);
        }

        #region Process(TagHelperContext context, TagHelperOutput output)

        [Fact]
        public void Process_AddsJsTreeClassAttribute()
        {
            helper.Process(null, output);

            Object actual = output.Attributes["class"].Value;
            Object expected = "js-tree";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_AppendsJsTreeClassAttribute()
        {
            output.Attributes.SetAttribute("class", "test");

            helper.Process(null, output);

            Object actual = output.Attributes["class"].Value;
            Object expected = "test js-tree";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_SetsForAttributeValue()
        {
            helper.Process(null, output);

            String actual = output.Content.GetContent();
            String expected =
                "<div class=\"js-tree-view-ids\">" +
                    "<input name=\"JsTree.SelectedIds\" type=\"hidden\" value=\"4567\" />" +
                    "<input name=\"JsTree.SelectedIds\" type=\"hidden\" value=\"12345\" />" +
                "</div>" +
                "<div class=\"js-tree-view\" for=\"JsTree.SelectedIds\">" +
                    "<ul>" +
                        "<li>Test" +
                            "<ul>" +
                                "<li id=\"12345\">Test1</li>" +
                                "<li id=\"23456\">Test2</li>" +
                            "</ul>" +
                        "</li>" +
                    "</ul>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
