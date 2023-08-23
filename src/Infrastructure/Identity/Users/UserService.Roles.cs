using Squares.Application.Common.Exceptions;
using Squares.Application.Identity.Roles;
using Microsoft.EntityFrameworkCore;

namespace Squares.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<List<RoleDto>> GetRolesAsync(int userId, CancellationToken token)
    {
        var user = await _userManager.Users
            .Include(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.User.Id == userId, token);

        if (user is null)
        {
            throw new NotFoundException(_localizer["Utente non trovato"]);
        }

        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(token);
        if (roles is null)
        {
            throw new NotFoundException(_localizer["Ruoli non trovati"]);
        }

        var userRoles = new List<Role>();
        foreach (var role in roles)
        {
            if (await _userManager.IsInRoleAsync(user, role.Name!))
            {
                userRoles.Add(role);
            }
        }

        return _mapper.Map<List<RoleDto>>(userRoles);
    }
}