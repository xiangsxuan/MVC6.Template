using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.Entity;
using MvcTemplate.Components.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Permission;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Services
{
    public class RoleServiceTests : IDisposable
    {
        private IAuthorizationProvider authorizationProvider;
        private TestingContext context;
        private RoleService service;
        private Role role;

        public RoleServiceTests()
        {
            context = new TestingContext();
            authorizationProvider = Substitute.For<IAuthorizationProvider>();
            service = Substitute.ForPartsOf<RoleService>(new UnitOfWork(context), authorizationProvider);

            context.DropData();
            SetUpData();
        }
        public void Dispose()
        {
            context.Dispose();
            service.Dispose();
        }

        #region Method: SeedPermissions(RoleView view)

        [Fact]
        public void SeedPermissions_FirstDepth()
        {
            RoleView view = new RoleView();
            service.SeedPermissions(view);

            IEnumerator<JsTreeNode> expected = CreatePermissions(role).Nodes.GetEnumerator();
            IEnumerator<JsTreeNode> actual = view.Permissions.Nodes.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPermissions_SecondDepth()
        {
            RoleView view = new RoleView();
            service.SeedPermissions(view);

            IEnumerator<JsTreeNode> expected = CreatePermissions(role).Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = view.Permissions.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPermissions_ThirdDepth()
        {
            RoleView view = new RoleView();
            service.SeedPermissions(view);

            IEnumerator<JsTreeNode> expected = CreatePermissions(role).Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = view.Permissions.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPermissions_BranchesWithoutId()
        {
            RoleView view = new RoleView();
            service.SeedPermissions(view);

            IEnumerable<JsTreeNode> nodes = view.Permissions.Nodes;
            IEnumerable<JsTreeNode> branches = GetAllBranchNodes(nodes);

            Assert.Empty(branches.Where(branch => branch.Id != null));
        }

        [Fact]
        public void SeedPermissions_LeafsWithId()
        {
            RoleView view = new RoleView();
            service.SeedPermissions(view);

            IEnumerable<JsTreeNode> nodes = view.Permissions.Nodes;
            IEnumerable<JsTreeNode> leafs = GetAllLeafNodes(nodes);

            Assert.Empty(leafs.Where(leaf => leaf.Id == null));
        }

        #endregion

        #region Method: GetViews()

        [Fact]
        public void GetViews_ReturnsRoleViews()
        {
            IEnumerator<RoleView> actual = service.GetViews().GetEnumerator();
            IEnumerator<RoleView> expected = context
                .Set<Role>()
                .ProjectTo<RoleView>()
                .OrderByDescending(view => view.CreationDate)
                .GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Permissions.SelectedIds, actual.Current.Permissions.SelectedIds);
                Assert.Equal(expected.Current.CreationDate, actual.Current.CreationDate);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Id, actual.Current.Id);
            }
        }

        #endregion

        #region Method: GetView(String id)

        [Fact]
        public void GetView_NoRole_ReturnsNull()
        {
            Assert.Null(service.GetView(""));
        }

        [Fact]
        public void GetView_ReturnsViewById()
        {
            service.When(sub => sub.SeedPermissions(Arg.Any<RoleView>())).DoNotCallBase();

            RoleView expected = Mapper.Map<RoleView>(role);
            RoleView actual = service.GetView(role.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.NotEmpty(actual.Permissions.SelectedIds);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void GetView_SetsSelectedIds()
        {
            service.When(sub => sub.SeedPermissions(Arg.Any<RoleView>())).DoNotCallBase();

            IEnumerable<String> expected = role.RolePermissions.Select(rolePermission => rolePermission.PermissionId);
            IEnumerable<String> actual = service.GetView(role.Id).Permissions.SelectedIds;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetView_SeedsPermissions()
        {
            service.When(sub => sub.SeedPermissions(Arg.Any<RoleView>())).DoNotCallBase();

            RoleView view = service.GetView(role.Id);

            service.Received().SeedPermissions(view);
        }

        #endregion

        #region Method: Create(RoleView view)

        [Fact]
        public void Create_Role()
        {
            RoleView view = ObjectFactory.CreateRoleView(2);

            service.Create(view);

            Role actual = context.Set<Role>().AsNoTracking().Single(role => role.Id == view.Id);
            RoleView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Create_RolePermissions()
        {
            RoleView view = ObjectFactory.CreateRoleView(2);
            view.Permissions = CreatePermissions(role);

            service.Create(view);

            IEnumerable<String> expected = view.Permissions.SelectedIds.OrderBy(permissionId => permissionId);
            IEnumerable<String> actual = context
                .Set<RolePermission>()
                .AsNoTracking()
                .Where(rolePermission => rolePermission.RoleId == view.Id)
                .Select(rolePermission => rolePermission.PermissionId)
                .OrderBy(permissionId => permissionId);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView view)

        [Fact]
        public void Edit_Role()
        {
            role = context.Set<Role>().AsNoTracking().Single();
            RoleView view = Mapper.Map<RoleView>(role);
            view.Title = role.Title += "Test";

            service.Edit(view);

            Role actual = context.Set<Role>().AsNoTracking().Single();
            RoleView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Edit_RolePermissions()
        {
            Permission permission = ObjectFactory.CreatePermission(100);
            context.Add(permission);
            context.SaveChanges();

            RoleView view = ObjectFactory.CreateRoleView();
            view.Permissions = CreatePermissions(role);

            view.Permissions.SelectedIds.Add(permission.Id);
            view.Permissions.SelectedIds.RemoveAt(0);

            service.Edit(view);

            IEnumerable<String> actual = context.Set<RolePermission>().AsNoTracking().Select(rolePermission => rolePermission.PermissionId).OrderBy(permissionId => permissionId);
            IEnumerable<String> expected = view.Permissions.SelectedIds.OrderBy(permissionId => permissionId);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Edit_RefreshesAuthorization()
        {
            service.Edit(ObjectFactory.CreateRoleView());

            authorizationProvider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Fact]
        public void Delete_NullsAccountRoles()
        {
            Account account = ObjectFactory.CreateAccount();
            account.RoleId = role.Id;
            account.Role = null;

            context.Add(account);
            context.SaveChanges();

            service.Delete(role.Id);

            Assert.NotEmpty(context.Set<Account>().Where(model => model.Id == account.Id && model.RoleId == null));
        }

        [Fact]
        public void Delete_Role()
        {
            service.Delete(role.Id);

            Assert.Empty(context.Set<Role>());
        }

        [Fact]
        public void Delete_RolePermissions()
        {
            service.Delete(role.Id);

            Assert.Empty(context.Set<RolePermission>());
        }

        [Fact]
        public void Delete_RefreshesAuthorization()
        {
            service.Delete(role.Id);

            authorizationProvider.Received().Refresh();
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            context.Add(role = ObjectFactory.CreateRole());
            foreach (String controller in new[] { "Roles", "Profile" })
                foreach (String action in new[] { "Edit", "Delete" })
                {
                    RolePermission rolePermission = ObjectFactory.CreateRolePermission(role.RolePermissions.Count + 1);
                    rolePermission.Permission.Area = controller == "Roles" ? "Administration" : null;
                    rolePermission.Permission.Controller = controller;
                    rolePermission.Permission.Action = action;
                    rolePermission.RoleId = role.Id;
                    rolePermission.Role = null;

                    role.RolePermissions.Add(rolePermission);
                    context.Add(rolePermission.Permission);
                }

            context.SaveChanges();
        }

        private JsTree CreatePermissions(Role role)
        {
            JsTreeNode rootNode = new JsTreeNode(Titles.All);
            JsTree expectedTree = new JsTree();

            expectedTree.Nodes.Add(rootNode);
            expectedTree.SelectedIds = role.RolePermissions.Select(rolePermission => rolePermission.PermissionId).ToList();

            IEnumerable<Permission> permissions = role
                .RolePermissions
                .Select(rolePermission => rolePermission.Permission)
                .Select(permission => new Permission
                {
                    Id = permission.Id,
                    Area = ResourceProvider.GetPermissionAreaTitle(permission.Area),
                    Controller = ResourceProvider.GetPermissionControllerTitle(permission.Area, permission.Controller),
                    Action = ResourceProvider.GetPermissionActionTitle(permission.Area, permission.Controller, permission.Action)
                });

            foreach (IGrouping<String, Permission> area in permissions.GroupBy(permission => permission.Area).OrderBy(permission => permission.Key ?? permission.FirstOrDefault().Controller))
            {
                JsTreeNode areaNode = new JsTreeNode(area.Key);
                foreach (IGrouping<String, Permission> controller in area.GroupBy(permission => permission.Controller).OrderBy(permission => permission.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controller.Key);
                    foreach (Permission permission in controller.OrderBy(permission => permission.Action))
                        controllerNode.Nodes.Add(new JsTreeNode(permission.Id, permission.Action));

                    if (areaNode.Title == null)
                        rootNode.Nodes.Add(controllerNode);
                    else
                        areaNode.Nodes.Add(controllerNode);
                }

                if (areaNode.Title != null)
                    rootNode.Nodes.Add(areaNode);
            }

            return expectedTree;
        }

        private IEnumerable<JsTreeNode> GetAllLeafNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> leafs = nodes.Where(node => node.Nodes.Count == 0).ToList();
            IEnumerable<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0);

            foreach (JsTreeNode branch in branches)
                leafs.AddRange(GetAllLeafNodes(branch.Nodes));

            return leafs;
        }
        private IEnumerable<JsTreeNode> GetAllBranchNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0).ToList();
            foreach (JsTreeNode branch in branches.ToArray())
                branches.AddRange(GetAllBranchNodes(branch.Nodes));

            return branches;
        }

        #endregion
    }
}
