using Microsoft.AspNetCore.Identity;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName, string createdBy)
        {
            var role = new ApplicationRole
            {
                Name = roleName,
                CreatedBy = createdBy,
                CreatedTime = DateTimeOffset.UtcNow
            };

            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(Guid roleId, string newRoleName, string updatedBy)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                role.Name = newRoleName;
                role.LastUpdatedBy = updatedBy;
                role.LastUpdatedTime = DateTimeOffset.UtcNow;
                return await _roleManager.UpdateAsync(role);
            }
            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
        }

        public async Task<IdentityResult> SoftDeleteRoleAsync(Guid roleId, string deletedBy)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                role.DeletedBy = deletedBy;
                role.DeletedTime = DateTimeOffset.UtcNow;
                return await _roleManager.UpdateAsync(role);
            }
            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
        }

        public async Task<IdentityResult> DeleteRoleAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                return await _roleManager.DeleteAsync(role);
            }
            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
        }

        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.ToList());
        }

        public async Task<ApplicationRole> FindRoleByIdAsync(Guid roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task<ApplicationRole> FindRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }
    }
}
