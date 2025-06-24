using Microsoft.AspNetCore.Mvc;
using Project3.Services;
using System.Security.Claims;

namespace Project3.Areas.System.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly UserRoleService _userRoleService;

        public NavigationMenuViewComponent(UserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var isAdminAccount = await _userRoleService.UserHasPermissionAsync(userId, "AdminAccount");
            var isAdminEstimates = await _userRoleService.UserHasPermissionAsync(userId, "AdminEstimates");
            var isAdminInsuranceProcess = await _userRoleService.UserHasPermissionAsync(userId, "AdminInsuranceProcess");
            var isAdminInsuranceProducts = await _userRoleService.UserHasPermissionAsync(userId, "AdminInsuranceProducts");
            var isAdminVehicleInformations = await _userRoleService.UserHasPermissionAsync(userId, "AdminVehicleInformations");
            var isAdminCompanyBillingPolicies = await _userRoleService.UserHasPermissionAsync(userId, "AdminCompanyBillingPolicies");
            var isAdminContactUs = await _userRoleService.UserHasPermissionAsync(userId, "AdminContactUs");
            var model = new NavigationMenuViewModel
            {
                IsAdmin = isAdmin,
                IsAdminAccount = isAdminAccount,
                IsAdminEstimates = isAdminEstimates,
                IsAdminInsuranceProcess = isAdminInsuranceProcess,
                IsAdminInsuranceProducts = isAdminInsuranceProducts,
                IsAdminVehicleInformations = isAdminVehicleInformations,
                IsAdminCompanyBillingPolicies = isAdminCompanyBillingPolicies,
                IsAdminContactUs = isAdminContactUs
            };
            return View(model);
        }
    }
    public class NavigationMenuViewModel
    {
        public bool IsAdmin { get; set; }

        public bool IsAdminAccount { get; set; }
        public bool IsAdminEstimates { get; set; }
        public bool IsAdminInsuranceProcess { get; set; }
        public bool IsAdminInsuranceProducts { get; set; }
        public bool IsAdminVehicleInformations { get; set; }
        public bool IsAdminCompanyBillingPolicies { get; set; }
        public bool IsAdminContactUs { get; set; }
    }

}
