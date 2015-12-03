using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.Entity;
using MvcTemplate.Components.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Privilege;
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

        #region Method: SeedPrivilegesTree(RoleView view)

        [Fact]
        public void SeedPrivilegesTree_FirstDepth()
        {
            RoleView view = new RoleView();
            service.SeedPrivilegesTree(view);

            IEnumerator<JsTreeNode> expected = CreatePrivilegesTree(role).Nodes.GetEnumerator();
            IEnumerator<JsTreeNode> actual = view.PrivilegesTree.Nodes.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPrivilegesTree_SecondDepth()
        {
            RoleView view = new RoleView();
            service.SeedPrivilegesTree(view);

            IEnumerator<JsTreeNode> expected = CreatePrivilegesTree(role).Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = view.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPrivilegesTree_ThirdDepth()
        {
            RoleView view = new RoleView();
            service.SeedPrivilegesTree(view);

            IEnumerator<JsTreeNode> expected = CreatePrivilegesTree(role).Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = view.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Id, actual.Current.Id);
                Assert.Equal(expected.Current.Title, actual.Current.Title);
                Assert.Equal(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Fact]
        public void SeedPrivilegesTree_BranchesWithoutId()
        {
            RoleView view = new RoleView();
            service.SeedPrivilegesTree(view);

            IEnumerable<JsTreeNode> nodes = view.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> branches = GetAllBranchNodes(nodes);

            Assert.Empty(branches.Where(branch => branch.Id != null));
        }

        [Fact]
        public void SeedPrivilegesTree_LeafsWithId()
        {
            RoleView view = new RoleView();
            service.SeedPrivilegesTree(view);

            IEnumerable<JsTreeNode> nodes = view.PrivilegesTree.Nodes;
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
                Assert.Equal(expected.Current.PrivilegesTree.SelectedIds, actual.Current.PrivilegesTree.SelectedIds);
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
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();

            RoleView expected = Mapper.Map<RoleView>(role);
            RoleView actual = service.GetView(role.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.NotEmpty(actual.PrivilegesTree.SelectedIds);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void GetView_SetsSelectedIds()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();

            IEnumerable<String> expected = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId);
            IEnumerable<String> actual = service.GetView(role.Id).PrivilegesTree.SelectedIds;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetView_SeedsPrivilegesTree()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();

            RoleView view = service.GetView(role.Id);

            service.Received().SeedPrivilegesTree(view);
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
        public void Create_RolePrivileges()
        {
            RoleView view = ObjectFactory.CreateRoleView(2);
            view.PrivilegesTree = CreatePrivilegesTree(role);

            service.Create(view);

            IEnumerable<String> expected = view.PrivilegesTree.SelectedIds.OrderBy(privilegeId => privilegeId);
            IEnumerable<String> actual = context
                .Set<RolePrivilege>()
                .AsNoTracking()
                .Where(rolePrivilege => rolePrivilege.RoleId == view.Id)
                .Select(rolePrivilege => rolePrivilege.PrivilegeId)
                .OrderBy(privilegeId => privilegeId);

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
        public void Edit_RolePrivileges()
        {
            Privilege privilege = ObjectFactory.CreatePrivilege(100);
            context.Add(privilege);
            context.SaveChanges();

            RoleView view = ObjectFactory.CreateRoleView();
            view.PrivilegesTree = CreatePrivilegesTree(role);
            view.PrivilegesTree.SelectedIds.Add(privilege.Id);
            view.PrivilegesTree.SelectedIds.RemoveAt(0);

            service.Edit(view);

            IEnumerable<String> actual = context.Set<RolePrivilege>().AsNoTracking().Select(rolePriv => rolePriv.PrivilegeId).OrderBy(privilegeId => privilegeId);
            IEnumerable<String> expected = view.PrivilegesTree.SelectedIds.OrderBy(privilegeId => privilegeId);

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
        public void Delete_RolePrivileges()
        {
            service.Delete(role.Id);

            Assert.Empty(context.Set<RolePrivilege>());
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
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege(role.RolePrivileges.Count + 1);
                    rolePrivilege.Privilege.Area = controller == "Roles" ? "Administration" : null;
                    rolePrivilege.Privilege.Controller = controller;
                    rolePrivilege.Privilege.Action = action;
                    rolePrivilege.RoleId = role.Id;
                    rolePrivilege.Role = null;

                    role.RolePrivileges.Add(rolePrivilege);
                    context.Add(rolePrivilege.Privilege);
                }

            context.SaveChanges();
        }

        private JsTree CreatePrivilegesTree(Role role)
        {
            JsTreeNode rootNode = new JsTreeNode(Titles.All);
            JsTree expectedTree = new JsTree();

            expectedTree.Nodes.Add(rootNode);
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToList();

            IEnumerable<Privilege> privileges = role
                .RolePrivileges
                .Select(rolePriv => rolePriv.Privilege)
                .Select(privilege => new Privilege
                {
                    Id = privilege.Id,
                    Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                    Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Area, privilege.Controller),
                    Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Area, privilege.Controller, privilege.Action)
                });

            foreach (IGrouping<String, Privilege> area in privileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
            {
                JsTreeNode areaNode = new JsTreeNode(area.Key);
                foreach (IGrouping<String, Privilege> controller in area.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controller.Key);
                    foreach (Privilege privilege in controller.OrderBy(privilege => privilege.Action))
                        controllerNode.Nodes.Add(new JsTreeNode(privilege.Id, privilege.Action));

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
