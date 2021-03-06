﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fabric.Authorization.Domain.Models;
using Fabric.Authorization.Domain.Stores;

namespace Fabric.Authorization.Persistence.InMemory.Stores
{
    public class InMemoryRoleStore : InMemoryGenericStore<Role>, IRoleStore
    {
        public override async Task<Role> Add(Role model)
        {
            model.Id = Guid.NewGuid();
            if (model.ParentRole.HasValue && Dictionary.ContainsKey(model.ParentRole.ToString()))
            {
                var parentRole = Dictionary[model.ParentRole.ToString()];
                parentRole.ChildRoles.Add(model.Id);
            }
            return await base.Add(model);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await Exists(id.ToString());
        }

        public async Task<Role> Get(Guid id)
        {
            return await Get(id.ToString());
        }

        public Task<IEnumerable<Role>> GetRoles(string grain = null, string securableItem = null,
            string roleName = null)
        {
            var roles = Dictionary.Select(kvp => kvp.Value);
            if (!string.IsNullOrEmpty(grain))
            {
                roles = roles.Where(r => r.Grain == grain);
            }
            if (!string.IsNullOrEmpty(securableItem))
            {
                roles = roles.Where(r => r.SecurableItem == securableItem);
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                roles = roles.Where(r => r.Name == roleName);
            }
            return Task.FromResult(roles.Where(r => !r.IsDeleted));
        }

        public async Task<Role> AddPermissionsToRole(Role role, ICollection<Permission> allowPermissions, ICollection<Permission> denyPermissions)
        {
            foreach (var permission in allowPermissions)
            {
                role.Permissions.Add(permission);
            }

            foreach (var permission in denyPermissions)
            {
                role.DeniedPermissions.Add(permission);
            }

            await Update(role);
            return role;
        }

        public async Task<Role> RemovePermissionsFromRole(Role role, Guid[] permissionIds)
        {
            foreach (var permissionId in permissionIds)
            {
                var permission = role.Permissions.First(p => p.Id == permissionId);
                role.Permissions.Remove(permission);
            }

            await Update(role);
            return role;
        }

        public async Task RemovePermissionFromRoles(Guid permissionId, string grain, string securableItem = null)
        {
            var roles = await GetRoles(grain, securableItem);

            // TODO: candidate for batch update
            foreach (var role in roles)
            {
                if (role.Permissions != null && role.Permissions.Any(p => p.Id == permissionId))
                {
                    var permission = role.Permissions.First(p => p.Id == permissionId);
                    role.Permissions.Remove(permission);
                    await Update(role);
                }
            }
        }
    }
}