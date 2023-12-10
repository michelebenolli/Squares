using Squares.Application.Common.Exceptions;
using Squares.Application.Identity.Roles;
using Microsoft.EntityFrameworkCore;

namespace Squares.Infrastructure.Identity;
internal partial class UserService
{
    public async Task<List<RoleDto>> GetRolesAsync(int userId, CancellationToken token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) ??
            throw new NotFoundException(_localizer["Utente non trovato"]);

        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(token) ??
            throw new NotFoundException(_localizer["Ruoli non trovati"]);

        var userRoles = await _userManager.GetRolesAsync(user);

        roles = roles.Where(x => userRoles.Contains(x.Name!)).ToList();
        return _mapper.Map<List<RoleDto>>(roles);
    }
}