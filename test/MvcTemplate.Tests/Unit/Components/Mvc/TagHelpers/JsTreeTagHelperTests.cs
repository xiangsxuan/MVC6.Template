using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using MvcTemplate.Components.Html;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Unit.Components.Html;
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
            JsTreeView tree = new JsTreeView();
            tree.JsTree.SelectedIds.Add("12345");
            tree.JsTree.Nodes.Add(new JsTreeNode("Test"));
            tree.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("12345", "Test1"));
            tree.JsTree.Nodes[0].Nodes.Add(new JsTreeNode("23456", "Test2"));

            helper = new JsTreeTagHelper(HtmlHelperFactory.CreateHtmlHelper(tree));
            output = new TagHelperOutput("div", new TagHelperAttributeList());
            helper.For = new ModelExpression("JsTree", null);
            helper.ViewContext = helper.Html.ViewContext;
        }

        #region Method: Process(TagHelperContext context, TagHelperOutput output)

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
            output.Attributes["class"] = " test";
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
