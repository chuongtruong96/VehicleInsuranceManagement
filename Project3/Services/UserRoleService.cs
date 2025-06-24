using Microsoft.AspNetCore.Identity;
using Project3.Models;
using Project3.ModelsView.Identity;
using Microsoft.EntityFrameworkCore;

namespace Project3.Services
{
    public class UserRoleService
    {
        private readonly VehicleInsuranceManagementContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleService(VehicleInsuranceManagementContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            // Lấy RoleId từ bảng AspNetUserRoles
            var roleIds = await _dbContext.AspNetUserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            // Lấy tên quyền từ bảng AspNetRoles
            var roles = await _dbContext.AspNetRoles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            return roles;
        }

        public async Task<List<string>> GetPermissionsForUserAsync(string userId)
        {
            // Lấy RoleId từ bảng AspNetUserRoles
            var roleIds = await _dbContext.AspNetUserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            // Lấy permissions_id từ bảng Role_Permissions
            var permissionIds = await _dbContext.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.PermissionsId)
                .ToListAsync();

            // Lấy tên quyền từ bảng Permissions
            var permissionNames = await _dbContext.AspNetRoles
                .Where(p => permissionIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToListAsync();

            return permissionNames;
        }

        public async Task PrintUserRolesAndPermissionsAsync(string userId)
        {
            var roles = await GetUserRolesAsync(userId);
            var permissions = await GetPermissionsForUserAsync(userId);

            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine("Roles:");
            roles.ForEach(role => Console.WriteLine(role));
            Console.WriteLine("Permissions:");
            permissions.ForEach(permission => Console.WriteLine(permission));
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permissionName)
        {
            var permissions = await GetPermissionsForUserAsync(userId);
            return permissions.Contains(permissionName);
        }
    }
}
