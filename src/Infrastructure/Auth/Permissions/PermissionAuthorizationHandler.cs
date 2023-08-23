using Squares.Application.Identity.Users;
using Squares.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Squares.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserService _userService;

    public PermissionAuthorizationHandler(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        int userId = int.TryParse(context.User?.GetUserId(), out int id) ? id : 0;

        if (userId != 0 && await _userService.HasPermissionAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}