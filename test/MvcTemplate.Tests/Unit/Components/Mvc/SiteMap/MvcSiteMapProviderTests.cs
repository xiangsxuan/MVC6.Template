using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MvcTemplate.Components.Extensions;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class MvcSiteMapProviderTests
    {
        private IAuthorizationProvider authorizationProvider;
        private IDictionary<String, Object> route;
        private MvcSiteMapProvider siteMap;
        private MvcSiteMapParser parser;
        private IConfiguration config;
        private ViewContext context;

        static MvcSiteMapProviderTests()
        {
            IConfiguration config = ConfigurationFactory.Create();
            Directory.CreateDirectory(config["Application:Path"]);
            CreateSiteMap(Path.Combine(config["Application:Path"], config["SiteMap:Path"]));
        }
        public MvcSiteMapProviderTests()
        {
            parser = new MvcSiteMapParser();
            config = ConfigurationFactory.Create();
            context = HtmlHelperFactory.CreateHtmlHelper().ViewContext;
            authorizationProvider = Substitute.For<IAuthorizationProvider>();
            siteMap = new MvcSiteMapProvider(config, parser, authorizationProvider);

            route = context.RouteData.Values;
        }

        #region GetSiteMap(ViewContext context)

        [Fact]
        public void GetSiteMap_NullAuthorization_ReturnsAllNodes()
        {
            siteMap = new MvcSiteMapProvider(config, parser, null);

            MvcSiteMapNode[] actual = siteMap.GetSiteMap(context).ToArray();

            Assert.Single(actual);

            Assert.Null(actual[0].Action);
            Assert.Null(actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);

            Assert.Empty(actual[0].Children);

            Assert.Equal("Index", actual[0].Action);
            Assert.Equal("Accounts", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-user", actual[0].IconClass);

            Assert.Null(actual[1].Action);
            Assert.Equal("Roles", actual[1].Controller);
            Assert.Equal("Administration", actual[1].Area);
            Assert.Equal("fa fa-users", actual[1].IconClass);

            actual = actual[1].Children.ToArray();

            Assert.Single(actual);
            Assert.Empty(actual[0].Children);

            Assert.Equal("Create", actual[0].Action);
            Assert.Equal("Roles", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-file-o", actual[0].IconClass);
        }

        [Fact]
        public void GetSiteMap_ReturnsAuthorizedNodes()
        {
            authorizationProvider.IsAuthorizedFor(context.HttpContext.User.Id(), "Administration", "Accounts", "Index").Returns(true);

            MvcSiteMapNode[] actual = siteMap.GetSiteMap(context).ToArray();

            Assert.Single(actual);

            Assert.Null(actual[0].Action);
            Assert.Null(actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.Single(actual);

            Assert.Empty(actual[0].Children);

            Assert.Equal("Index", actual[0].Action);
            Assert.Equal("Accounts", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-user", actual[0].IconClass);
        }

        [Fact]
        public void GetSiteMap_SetsActiveMenu()
        {
            route["action"] = "Create";
            route["controller"] = "Roles";
            route["area"] = "Administration";

            siteMap = new MvcSiteMapProvider(config, parser, null);

            MvcSiteMapNode[] actual = siteMap.GetSiteMap(context).ToArray();

            Assert.Single(actual);
            Assert.False(actual[0].IsActive);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);
            Assert.False(actual[0].IsActive);
            Assert.False(actual[1].IsActive);
            Assert.Empty(actual[0].Children);

            actual = actual[1].Children.ToArray();

            Assert.Empty(actual[0].Children);
            Assert.True(actual[0].IsActive);
            Assert.Single(actual);
        }

        [Fact]
        public void GetSiteMap_NonMenuChildrenNodeIsActive_SetsActiveMenu()
        {
            route["action"] = "Edit";
            route["controller"] = "Accounts";
            route["area"] = "Administration";

            siteMap = new MvcSiteMapProvider(config, parser, null);

            MvcSiteMapNode[] actual = siteMap.GetSiteMap(context).ToArray();

            Assert.Single(actual);
            Assert.False(actual[0].IsActive);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);
            Assert.True(actual[0].IsActive);
            Assert.False(actual[1].IsActive);
            Assert.Empty(actual[0].Children);

            actual = actual[1].Children.ToArray();

            Assert.Single(actual);
            Assert.False(actual[0].IsActive);
            Assert.Empty(actual[0].Children);
        }

        [Fact]
        public void GetSiteMap_ActiveMenuParents_SetsHasActiveChildren()
        {
            route["action"] = "Create";
            route["controller"] = "Roles";
            route["area"] = "Administration";

            siteMap = new MvcSiteMapProvider(config, parser, null);

            MvcSiteMapNode[] actual = siteMap.GetSiteMap(context).ToArray();

            Assert.Single(actual);
            Assert.True(actual[0].HasActiveChildren);

            actual = actual[0].Children.ToArray();

            Assert.Equal(2, actual.Length);
            Assert.Empty(actual[0].Children);
            Assert.True(actual[1].HasActiveChildren);
            Assert.False(actual[0].HasActiveChildren);

            actual = actual[1].Children.ToArray();

            Assert.Single(actual);
            Assert.Empty(actual[0].Children);
            Assert.False(actual[0].HasActiveChildren);
        }

        [Fact]
        public void GetSiteMap_RemovesEmptyNodes()
        {
            authorizationProvider.IsAuthorizedFor(Arg.Any<Int32?>(), Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>()).Returns(true);
            authorizationProvider.IsAuthorizedFor(context.HttpContext.User.Id(), "Administration", "Roles", "Create").Returns(false);

            MvcSiteMapNode[] actual = siteMap.GetSiteMap(context).ToArray();

            Assert.Single(actual);

            Assert.Null(actual[0].Action);
            Assert.Null(actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-gears", actual[0].IconClass);

            actual = actual[0].Children.ToArray();

            Assert.Single(actual);

            Assert.Empty(actual[0].Children);

            Assert.Equal("Index", actual[0].Action);
            Assert.Equal("Accounts", actual[0].Controller);
            Assert.Equal("Administration", actual[0].Area);
            Assert.Equal("fa fa-user", actual[0].IconClass);
        }

        #endregion

        #region GetBreadcrumb(ViewContext context)

        [Fact]
        public void GetBreadcrumb_IsCaseInsensitive()
        {
            route["controller"] = "profile";
            route["action"] = "edit";
            route["area"] = null;

            MvcSiteMapNode[] actual = siteMap.GetBreadcrumb(context).ToArray();

            Assert.Equal(3, actual.Length);

            Assert.Equal("fa fa-home", actual[0].IconClass);
            Assert.Equal("Home", actual[0].Controller);
            Assert.Equal("Index", actual[0].Action);
            Assert.Null(actual[0].Area);

            Assert.Equal("fa fa-user", actual[1].IconClass);
            Assert.Equal("Profile", actual[1].Controller);
            Assert.Null(actual[1].Action);
            Assert.Null(actual[1].Area);

            Assert.Equal("fa fa-pencil", actual[2].IconClass);
            Assert.Equal("Profile", actual[2].Controller);
            Assert.Equal("Edit", actual[2].Action);
            Assert.Null(actual[2].Area);
        }

        [Fact]
        public void GetBreadcrumb_NoAction_ReturnsEmpty()
        {
            route["controller"] = "profile";
            route["action"] = "edit";
            route["area"] = "area";

            Assert.Empty(siteMap.GetBreadcrumb(context));
        }

        #endregion

        #region Test helpers

        private static void CreateSiteMap(String path)
        {
            XElement
                .Parse(
                    @"<siteMap>
                        <siteMapNode icon=""fa fa-home"" controller=""Home"" action=""Index"">
                            <siteMapNode icon=""fa fa-user"" controller=""Profile"">
                                <siteMapNode icon=""fa fa-pencil"" controller=""Profile"" action=""Edit"" />
                            </siteMapNode>
                            <siteMapNode menu=""true"" icon=""fa fa-gears"" area=""Administration"">
                                <siteMapNode menu=""true"" icon=""fa fa-user"" area=""Administration"" controller=""Accounts"" action=""Index"">
                                    <siteMapNode icon=""fa fa-info"" area=""Administration"" controller=""Accounts"" action=""Details"">
                                        <siteMapNode icon=""fa fa-pencil"" area=""Administration"" controller=""Accounts"" action=""Edit"" />
                                    </siteMapNode>
                                </siteMapNode>
                                <siteMapNode menu=""true"" icon=""fa fa-users"" area=""Administration"" controller=""Roles"">
                                    <siteMapNode menu=""true"" icon=""fa fa-file-o"" area=""Administration"" controller=""Roles"" action=""Create"" />
                                    <siteMapNode icon=""fa fa-pencil"" area=""Administration"" controller=""Roles"" action=""Edit"" />
                                </siteMapNode>
                            </siteMapNode>
                        </siteMapNode>
                    </siteMap>")
                .Save(path);
        }

        #endregion
    }
}
