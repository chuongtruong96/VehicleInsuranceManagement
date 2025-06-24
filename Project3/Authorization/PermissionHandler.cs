using Microsoft.AspNetCore.Authorization;
using Project3.Services;
using System.Security.Claims;

namespace Project3.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                context.Fail();
                return;
            }

            // Resolve UserRoleService from IServiceProvider
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRoleService = scope.ServiceProvider.GetRequiredService<UserRoleService>();
                var hasPermission = await userRoleService.UserHasPermissionAsync(userId, requirement.Permission);

                if (hasPermission)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }

    }
}
