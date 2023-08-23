using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Squares.Application.Common.Exceptions;
using Squares.Application.Common.Mailing;
using Squares.Application.Identity.Users.Requests;
using Squares.Domain.Common;
using Squares.Domain.Identity;
using Squares.Shared.Authorization;
using System.Security.Claims;

namespace Squares.Infrastructure.Identity;
internal partial class UserService
{
    public async Task<int> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? objectId = principal.GetObjectId();
        if (string.IsNullOrWhiteSpace(objectId))
        {
            throw new InternalServerException(_localizer["ObjectId non valido"]);
        }

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.ObjectId == objectId)
            ?? await CreateOrUpdateFromPrincipalAsync(principal);

        if (principal.FindFirstValue(ClaimTypes.Role) is string role &&
            await _roleManager.RoleExistsAsync(role) &&
            !await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? email = principal.FindFirstValue(ClaimTypes.Upn);
        string? username = principal.GetDisplayName();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            throw new InternalServerException(_localizer["Username o email non validi"]);
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
        {
            throw new InternalServerException(_localizer["Username già utilizzato"]);
        }

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
            {
                throw new InternalServerException(_localizer["Email già utilizzata"]);
            }
        }

        IdentityResult? result;
        if (user is not null)
        {
            user.ObjectId = principal.GetObjectId();
            result = await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
        else
        {
            user = new ApplicationUser
            {
                ObjectId = principal.GetObjectId(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
                User = new User
                {
                    UserName = username,
                    FirstName = principal.FindFirstValue(ClaimTypes.GivenName)!,
                    LastName = principal.FindFirstValue(ClaimTypes.Surname)!,
                    Email = email
                }
            };
            result = await _userManager.CreateAsync(user);
            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        }

        return !result.Succeeded ? throw new InternalServerException(_localizer["Si è verificato un errore"]) : user;
    }

    public async Task<int> CreateAsync(CreateUserRequest request, string origin)
    {
        var user = _mapper.Map<ApplicationUser>(request);
        user.UserName = Guid.NewGuid().ToString();
        user.User.UserName = user.UserName;

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Creazione non riuscita"]);
        }

        foreach (var role in await _roleManager.Roles.ToListAsync())
        {
            if (request.Roles?.Any(x => x == role.Id) == true)
            {
                await _userManager.AddToRoleAsync(user, role.Name!);
            }
        }

        await SendInvitationEmailAsync(user, origin);
        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        return user.Id;
    }

    public async Task UpdateAsync(UpdateUserRequest request, CancellationToken token)
    {
        var user = await _userManager.Users
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User.Id == request.Id, token);

        _ = user ?? throw new NotFoundException(_localizer["L'utente non è stato trovato"]);
        _mapper.Map(request, user);

        string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        string? email = await _userManager.GetEmailAsync(user);
        if (request.Email != email)
        {
            await _userManager.SetEmailAsync(user, request.Email);
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Operazione non riuscita"]);
        }

        // Eventually update user roles
        var roles = await _roleManager.Roles.ToListAsync(token);
        var userRoles = await _userManager.GetRolesAsync(user);
        var roleNames = roles.Where(x => request.Roles?.Contains(x.Id) == true).Select(x => x.Name!);

        // Check if the given roles are different from the previous ones
        if (!userRoles.All(roleNames.Contains) || userRoles.Count != roleNames.Count())
        {
            var adminRole = roles.Find(x => x.Name == AppRoles.Admin);

            // Check if the user is an admin for which the admin role is getting disabled
            if (userRoles.Contains(AppRoles.Admin) && !request.Roles?.Contains(adminRole!.Id) == true)
            {
                var admins = await _userManager.GetUsersInRoleAsync(AppRoles.Admin);
                if (admins.Count == 1)
                {
                    throw new ConflictException(_localizer["Il tenant deve avere almeno un amministratore"]);
                }
            }

            // Add and remove user roles
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, roleNames);
            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));
        }
        else
        {
            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
    }

    public async Task<int> RegisterAsync(RegisterUserRequest request, string origin)
    {
        var user = _mapper.Map<ApplicationUser>(request);

        string userName = Guid.NewGuid().ToString();
        user.UserName = userName;
        user.User.UserName = userName;

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Operazione non riuscita"]);
        }

        await _userManager.AddToRoleAsync(user, AppRoles.Basic);

        if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            await SendInvitationEmailAsync(user, origin);
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        return user.Id;
    }
}
