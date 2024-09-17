using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnDemandTutor.Contract.Repositories.Entity;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(string roleName, string createdBy);
        Task<IdentityResult> UpdateRoleAsync(Guid roleId, string newRoleName, string updatedBy);
        Task<IdentityResult> SoftDeleteRoleAsync(Guid roleId, string deletedBy);
        Task<IdentityResult> DeleteRoleAsync(Guid roleId);
        Task<List<ApplicationRole>> GetAllRolesAsync();
        Task<ApplicationRole> FindRoleByIdAsync(Guid roleId);
        Task<ApplicationRole> FindRoleByNameAsync(string roleName);
    }
}
