using Microsoft.AspNetCore.Authorization;
using Squares.Shared.Authorization;

namespace Squares.Infrastructure.Auth.Permissions;
public class PermissionAttribute : AuthorizeAttribute
{
    public PermissionAttribute(string action, string resource)
    {
        Policy = AppPermission.GetName(action, resource);
    }
}