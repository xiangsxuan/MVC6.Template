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
        private TestingContext context;
        private RoleService service;

        public RoleServiceTests()
        {
            context = new TestingContext();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            service = Substitute.ForPartsOf<RoleService>(new UnitOfWork(context));

            context.DropData();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
            context.Dispose();
            service.Dispose();
        }

        #region Method: SeedPrivilegesTree(RoleView view)

        [Fact]
        public void SeedPrivilegesTree_FirstDepth()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.GetEnumerator();

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
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

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
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

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
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerable<JsTreeNode> nodes = role.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> branches = GetAllBranchNodes(nodes);

            Assert.Empty(branches.Where(branch => branch.Id != null));
        }

        [Fact]
        public void SeedPrivilegesTree_LeafsWithId()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerable<JsTreeNode> nodes = role.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> leafs = GetAllLeafNodes(nodes);

            Assert.Empty(leafs.Where(leaf => leaf.Id == null));
        }

        #endregion

        #region Method: GetViews()

        [Fact]
        public void GetViews_ReturnsRoleViews()
        {
            context.Set<Role>().Add(ObjectFactory.CreateRole(1));
            context.Set<Role>().Add(ObjectFactory.CreateRole(2));
            context.SaveChanges();

            IEnumerator<RoleView> actual = service.GetViews().GetEnumerator();
            IEnumerator<RoleView> expected = context
                .Set<Role>()
                .ProjectTo<RoleView>()
                .OrderByDescending(role => role.CreationDate)
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
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView expected = Mapper.Map<RoleView>(role);
            RoleView actual = service.GetView(role.Id);

            Assert.Equal(expected.PrivilegesTree.SelectedIds, actual.PrivilegesTree.SelectedIds);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void GetView_SetsSelectedIds()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = CreateRoleWithPrivileges();
            using (TestingContext testingContext = new TestingContext())
            {
                testingContext.Set<Role>().Add(role, GraphBehavior.SingleObject);
                testingContext.SaveChanges();

                foreach (RolePrivilege rolePriv in role.RolePrivileges)
                    testingContext.Set<Privilege>().Add(rolePriv.Privilege);
                foreach (RolePrivilege rolePriv in role.RolePrivileges)
                    testingContext.Set<RolePrivilege>().Add(rolePriv);
                testingContext.SaveChanges();
            }

            IEnumerable<String> expected = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId);
            IEnumerable<String> actual = service.GetView(role.Id).PrivilegesTree.SelectedIds;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetView_SeedsPrivilegesTree()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = service.GetView(role.Id);

            service.Received().SeedPrivilegesTree(roleView);
        }

        #endregion

        #region Method: Create(RoleView view)

        [Fact]
        public void Create_Role()
        {
            RoleView view = ObjectFactory.CreateRoleView();

            service.Create(view);

            Role actual = context.Set<Role>().AsNoTracking().SingleOrDefault();
            RoleView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Create_RolePrivileges()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            service.Create(CreateRoleView());

            IEnumerable<String> expected = privileges.Select(privilege => privilege.Id).OrderBy(privilegeId => privilegeId);
            IEnumerable<String> actual = context
                .Set<RolePrivilege>()
                .AsNoTracking()
                .Select(role => role.PrivilegeId)
                .OrderBy(privilegeId => privilegeId);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView view)

        [Fact]
        public void Edit_Role()
        {
            Role role = ObjectFactory.CreateRole();
            using (TestingContext testingContext = new TestingContext())
            {
                testingContext.Set<Role>().Add(role);
                testingContext.SaveChanges();
            }

            RoleView roleView = Mapper.Map<RoleView>(role);
            roleView.Title = role.Title += "Test";

            service.Edit(roleView);

            Role actual = context.Set<Role>().AsNoTracking().Single();
            RoleView expected = roleView;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Edit_RolePrivileges()
        {
            Role role = CreateRoleWithPrivileges();
            using (TestingContext testingContext = new TestingContext())
            {
                testingContext.Set<Role>().Add(role, GraphBehavior.SingleObject);
                testingContext.SaveChanges();

                foreach (RolePrivilege rolePriv in role.RolePrivileges)
                    testingContext.Set<Privilege>().Add(rolePriv.Privilege);
                foreach (RolePrivilege rolePriv in role.RolePrivileges)
                    testingContext.Set<RolePrivilege>().Add(rolePriv);
                testingContext.SaveChanges();
            }

            RoleView roleView = CreateRoleView();
            roleView.PrivilegesTree.SelectedIds.RemoveAt(0);

            service.Edit(roleView);

            IEnumerable<String> actual = context.Set<RolePrivilege>().AsNoTracking().Select(rolePriv => rolePriv.PrivilegeId);
            IEnumerable<String> expected = CreateRoleView().PrivilegesTree.SelectedIds.Skip(1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Edit_RefreshesAuthorization()
        {
            using (TestingContext context = new TestingContext())
            {
                Role role = ObjectFactory.CreateRole();
                context.Set<Role>().Add(role);
                context.SaveChanges();
            }

            service.Edit(ObjectFactory.CreateRoleView());

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Fact]
        public void Delete_NullsAccountRoles()
        {
            Account account = ObjectFactory.CreateAccount();
            Role role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;
            account.Role = role;

            context.Set<Role>().Add(account.Role);
            context.Set<Account>().Add(account);
            context.SaveChanges();

            service.Delete(role.Id);

            Assert.NotEmpty(context.Set<Account>().Where(acc => acc.Id == account.Id && acc.RoleId == null));
        }

        [Fact]
        public void Delete_Role()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Delete(role.Id);

            Assert.Empty(context.Set<Role>());
        }

        [Fact]
        public void Delete_RolePrivileges()
        {
            RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
            context.Set<Privilege>().Add(rolePrivilege.Privilege);
            context.Set<RolePrivilege>().Add(rolePrivilege);
            context.Set<Role>().Add(rolePrivilege.Role);
            context.SaveChanges();

            service.Delete(rolePrivilege.RoleId);

            Assert.Empty(context.Set<RolePrivilege>());
        }

        [Fact]
        public void Delete_RefreshesAuthorization()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Delete(role.Id);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Test helpers

        private JsTree CreatePrivilegesTree(Role role)
        {
            JsTreeNode rootNode = new JsTreeNode(Titles.All);
            JsTree expectedTree = new JsTree();

            expectedTree.Nodes.Add(rootNode);
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToList();

            IEnumerable<Privilege> allPrivileges = role
                .RolePrivileges
                .Select(rolePriv => rolePriv.Privilege)
                .Select(privilege => new Privilege
                {
                    Id = privilege.Id,
                    Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                    Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Area, privilege.Controller),
                    Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Area, privilege.Controller, privilege.Action)
                });

            foreach (IGrouping<String, Privilege> areaPrivilege in allPrivileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
            {
                JsTreeNode areaNode = new JsTreeNode(areaPrivilege.Key);
                foreach (IGrouping<String, Privilege> controllerPrivilege in areaPrivilege.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controllerPrivilege.Key);
                    foreach (IGrouping<String, Privilege> actionPrivilege in controllerPrivilege.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
                        controllerNode.Nodes.Add(new JsTreeNode(actionPrivilege.First().Id, actionPrivilege.Key));

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
        private Role CreateRoleWithPrivileges()
        {
            Int32 privilegeNumber = 1;
            Role role = ObjectFactory.CreateRole();
            role.RolePrivileges = new List<RolePrivilege>();

            foreach (String controller in new[] { "Roles", "Profile" })
                foreach (String action in new[] { "Edit", "Delete" })
                {
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege(privilegeNumber++);
                    rolePrivilege.Privilege.Area = controller == "Roles" ? "Administration" : null;
                    rolePrivilege.Privilege.Controller = controller;
                    rolePrivilege.Privilege.Action = action;
                    rolePrivilege.RoleId = role.Id;
                    rolePrivilege.Role = role;

                    role.RolePrivileges.Add(rolePrivilege);
                }

            return role;
        }
        private RoleView CreateRoleView()
        {
            Role role = CreateRoleWithPrivileges();
            RoleView roleView = Mapper.Map<RoleView>(role);
            roleView.PrivilegesTree = CreatePrivilegesTree(role);

            return roleView;
        }

        private IEnumerable<JsTreeNode> GetAllBranchNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0).ToList();
            foreach (JsTreeNode branch in branches.ToArray())
                branches.AddRange(GetAllBranchNodes(branch.Nodes));

            return branches;
        }
        private IEnumerable<JsTreeNode> GetAllLeafNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> leafs = nodes.Where(node => node.Nodes.Count == 0).ToList();
            IEnumerable<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0);

            foreach (JsTreeNode branch in branches)
                leafs.AddRange(GetAllLeafNodes(branch.Nodes));

            return leafs;
        }

        #endregion
    }
}
