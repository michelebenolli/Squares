using Squares.Application.Common.Events;
using Squares.Application.Identity.Users;
using Squares.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Squares.Infrastructure.Identity;

namespace Squares.Infrastructure.Identity;

internal class InvalidateUserPermissionCacheHandler :
    IEventNotificationHandler<ApplicationUserUpdatedEvent>,
    IEventNotificationHandler<ApplicationRoleUpdatedEvent>
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public InvalidateUserPermissionCacheHandler(IUserService userService, UserManager<ApplicationUser> userManager)
    {
        (_userService, _userManager) = (userService, userManager);
    }

    public async Task Handle(EventNotification<ApplicationUserUpdatedEvent> notification, CancellationToken token)
    {
        if (notification.Event.RolesUpdated)
        {
            await _userService.InvalidatePermissionCacheAsync(notification.Event.UserId, token);
        }
    }

    public async Task Handle(EventNotification<ApplicationRoleUpdatedEvent> notification, CancellationToken token)
    {
        if (notification.Event.PermissionsUpdated)
        {
            foreach (var user in await _userManager.GetUsersInRoleAsync(notification.Event.RoleName))
            {
                await _userService.InvalidatePermissionCacheAsync(user.Id, token);
            }
        }
    }
}