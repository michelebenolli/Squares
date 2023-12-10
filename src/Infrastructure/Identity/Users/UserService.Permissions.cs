using Squares.Application.Common.Caching;
using Squares.Application.Common.Exceptions;
using Squares.Shared.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Squares.Infrastructure.Identity;
internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(int userId, CancellationToken token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        _ = user ?? throw new NotFoundException(_localizer["Autenticazione non riuscita"]);

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        var roles = await _roleManager.Roles.Where(x => userRoles.Contains(x.Name!)).ToListAsync(token);

        foreach (var role in roles)
        {
            permissions.AddRange(await _db.RoleClaims
                .Where(x => x.RoleId == role.Id && x.ClaimType == AppClaims.Permission)
                .Select(x => x.ClaimValue!)
                .ToListAsync(token));
        }

        return permissions.Distinct().ToList();
    }

    // TODO: Remove?
    // TODO: Complete, get the permissions and call it from the controller.
    public async Task<List<string>> GetPermissionsAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        _ = user ?? throw new NotFoundException(_localizer["Utente non trovato"]);

        // TODO: Not working because claims are null, try getting the user roles and then
        // the related claims...?
        var claims = await _userManager.GetClaimsAsync(user);

        // TODO: Is this filter correct?, test
        return claims.Where(x => x.Type == AppClaims.Permission).Select(x => x.Value).ToList();
    }

    public async Task<bool> HasPermissionAsync(int userId, string permission, CancellationToken token)
    {
        var permissions = await _cache.GetOrSetAsync(
            _cacheKeys.GetCacheKey(AppClaims.Permission, userId),
            () => GetPermissionsAsync(userId, token),
            token: token);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(int userId, CancellationToken token) =>
        _cache.RemoveAsync(_cacheKeys.GetCacheKey(AppClaims.Permission, userId), token);
}