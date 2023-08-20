using Microsoft.AspNetCore.Authorization;
using Squares.Shared.Authorization;

namespace Squares.Infrastructure.Auth.Permissions;
public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = FSHPermission.NameFor(action, resource);
}