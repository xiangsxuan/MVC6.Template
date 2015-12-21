using MvcTemplate.Components.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Permission;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private IAuthorizationProvider AuthorizationProvider { get; }

        public RoleService(IUnitOfWork unitOfWork, IAuthorizationProvider provider)
            : base(unitOfWork)
        {
            AuthorizationProvider = provider;
        }

        public virtual void SeedPermissions(RoleView view)
        {
            JsTreeNode root = new JsTreeNode(Titles.All);
            view.Permissions.Nodes.Add(root);

            IEnumerable<Permission> permissions = GetAllPermissions();
            foreach (IGrouping<String, Permission> area in permissions.GroupBy(permission => permission.Area))
            {
                JsTreeNode areaNode = new JsTreeNode(area.Key);
                foreach (IGrouping<String, Permission> controller in area.GroupBy(permission => permission.Controller).OrderBy(permission => permission.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controller.Key);
                    foreach (Permission permission in controller)
                        controllerNode.Nodes.Add(new JsTreeNode(permission.Id, permission.Action));

                    if (areaNode.Title == null)
                        root.Nodes.Add(controllerNode);
                    else
                        areaNode.Nodes.Add(controllerNode);
                }

                if (areaNode.Title != null)
                    root.Nodes.Add(areaNode);
            }
        }
        private IEnumerable<Permission> GetAllPermissions()
        {
            return UnitOfWork
                .Select<Permission>()
                .ToArray()
                .Select(permission => new Permission
                {
                    Id = permission.Id,
                    Area = ResourceProvider.GetPermissionAreaTitle(permission.Area),
                    Controller = ResourceProvider.GetPermissionControllerTitle(permission.Area, permission.Controller),
                    Action = ResourceProvider.GetPermissionActionTitle(permission.Area, permission.Controller, permission.Action)
                })
                .OrderBy(permission => permission.Area ?? permission.Controller)
                .ThenBy(permission => permission.Controller)
                .ThenBy(permission => permission.Action);
        }

        public IQueryable<RoleView> GetViews()
        {
            return UnitOfWork
                .Select<Role>()
                .To<RoleView>()
                .OrderByDescending(role => role.CreationDate);
        }
        public RoleView GetView(String id)
        {
            RoleView role = UnitOfWork.GetAs<Role, RoleView>(id);
            if (role != null)
            {
                role.Permissions.SelectedIds = UnitOfWork
                    .Select<RolePermission>()
                    .Where(rolePermission => rolePermission.RoleId == role.Id)
                    .Select(rolePermission => rolePermission.PermissionId)
                    .ToList();

                SeedPermissions(role);
            }

            return role;
        }

        public void Create(RoleView view)
        {
            CreateRole(view);
            CreateRolePermissions(view);

            UnitOfWork.Commit();
        }
        public void Edit(RoleView view)
        {
            Role role = UnitOfWork.Get<Role>(view.Id);
            EditRolePermissions(role, view);
            EditRole(role, view);

            UnitOfWork.Commit();

            AuthorizationProvider.Refresh();
        }
        public void Delete(String id)
        {
            RemoveRoleFromAccounts(id);
            DeleteRolePermissions(id);
            DeleteRole(id);

            UnitOfWork.Commit();

            AuthorizationProvider.Refresh();
        }

        private void CreateRole(RoleView view)
        {
            Role role = UnitOfWork.To<Role>(view);

            UnitOfWork.Insert(role);
        }
        private void CreateRolePermissions(RoleView view)
        {
            foreach (String permissionId in view.Permissions.SelectedIds)
                UnitOfWork.Insert(new RolePermission { RoleId = view.Id, PermissionId = permissionId });
        }

        private void EditRole(Role role, RoleView view)
        {
            role.Title = view.Title;

            UnitOfWork.Update(role);
        }
        private void EditRolePermissions(Role role, RoleView view)
        {
            List<String> selectedPermissions = view.Permissions.SelectedIds.ToList();
            RolePermission[] rolePermissions = UnitOfWork.Select<RolePermission>().Where(rolePermission => rolePermission.RoleId == role.Id).ToArray();

            foreach (RolePermission rolePermission in rolePermissions)
                if (!selectedPermissions.Remove(rolePermission.PermissionId))
                    UnitOfWork.Delete(rolePermission);

            foreach (String permissionId in selectedPermissions)
                UnitOfWork.Insert(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
        }

        private void DeleteRole(String id)
        {
            UnitOfWork.Delete<Role>(id);
        }
        private void DeleteRolePermissions(String roleId)
        {
            IQueryable<RolePermission> rolePermissions = UnitOfWork
                .Select<RolePermission>()
                .Where(rolePermission => rolePermission.RoleId == roleId);

            foreach (RolePermission rolePermission in rolePermissions)
                UnitOfWork.Delete(rolePermission);
        }
        private void RemoveRoleFromAccounts(String roleId)
        {
            IQueryable<Account> accountsWithRole = UnitOfWork
                .Select<Account>()
                .Where(account => account.RoleId == roleId);

            foreach (Account account in accountsWithRole)
            {
                account.RoleId = null;
                UnitOfWork.Update(account);
            }
        }
    }
}
