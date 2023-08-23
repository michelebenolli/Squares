using AutoMapper;
using Finbuckle.MultiTenant;
using Squares.Application.Common.Events;
using Squares.Application.Common.Exceptions;
using Squares.Application.Common.Interfaces;
using Squares.Application.Common.Models;
using Squares.Application.Identity.Roles;
using Squares.Application.Identity.Roles.Requests;
using Squares.Domain.Identity;
using Squares.Infrastructure.Persistence.Context;
using Squares.Shared.Authorization;
using Squares.Shared.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using X.PagedList;

namespace Squares.Infrastructure.Identity;
internal class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly DatabaseContext _db;
    private readonly IStringLocalizer<RoleService> _localizer;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantInfo _currentTenant;
    private readonly IEventPublisher _events;
    private readonly IMapper _mapper;

    public RoleService(
        RoleManager<Role> roleManager,
        DatabaseContext db,
        IStringLocalizer<RoleService> localizer,
        ICurrentUser currentUser,
        ITenantInfo currentTenant,
        IEventPublisher events,
        IMapper mapper)
    {
        _roleManager = roleManager;
        _db = db;
        _localizer = localizer;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _events = events;
        _mapper = mapper;
    }

    public async Task<RoleDto> GetByIdAsync(int id)
    {
        var role = await _db.Roles.SingleOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<IPagedList<RoleDto>> SearchAsync(PagedRequest request, CancellationToken token)
    {
        var roles = await _roleManager.Roles.ToPagedListAsync(request.PageNumber, request.PageSize, token);
        return _mapper.Map<IPagedList<RoleDto>>(roles);
    }

    public async Task<List<RoleDto>> ListAsync(CancellationToken token)
    {
        var roles = await _roleManager.Roles.ToListAsync(token);
        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<int> GetCountAsync(CancellationToken token)
    {
        return await _roleManager.Roles.CountAsync(token);
    }

    public async Task<bool> ExistsAsync(string name, int? excludeId = null)
    {
        return await _roleManager.FindByNameAsync(name) is Role role && role.Id != excludeId;
    }

    public async Task<int> CreateAsync(CreateRoleRequest request)
    {
        var role = new Role(request.Name, request.Description);
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Creazione del ruolo non riuscita"]);
        }

        await _events.PublishAsync(new ApplicationRoleCreatedEvent(role.Id, role.Name!));
        return role.Id;
    }

    public async Task UpdateAsync(UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        _ = role ?? throw new NotFoundException(_localizer["Ruolo non trovato"]);

        if (AppRoles.IsDefault(role.Name!))
        {
            throw new ConflictException(_localizer["Il ruolo non può essere modificato"]);
        }

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpperInvariant();
        role.Description = request.Description;
        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Modifica del ruolo non riuscita"]);
        }

        await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name!));
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        _ = role ?? throw new NotFoundException(_localizer["Ruolo non trovato"]);

        if (AppRoles.IsDefault(role.Name!))
        {
            throw new ConflictException(_localizer["Il ruolo non può essere eliminato"]);
        }

        await _roleManager.DeleteAsync(role);
        await _events.PublishAsync(new ApplicationRoleDeletedEvent(role.Id, role.Name!));
    }

    public List<string> GetPermissions()
    {
        var permissions = _currentTenant.Id == MultitenancyConstants.Root.Id ? AppPermissions.All : AppPermissions.Admin;
        return permissions.Select(x => x.Name).ToList();
    }

    public async Task<List<string>> GetPermissionsAsync(int roleId)
    {
        return await _db.RoleClaims
            .Where(x => x.RoleId == roleId && x.ClaimType == AppClaims.Permission)
            .Select(x => x.ClaimValue!)
            .ToListAsync();
    }

    public async Task UpdatePermissionsAsync(UpdatePermissionsRequest request, CancellationToken token)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        _ = role ?? throw new NotFoundException(_localizer["Ruolo non trovato"]);

        if (role.Name == AppRoles.Admin)
        {
            throw new ConflictException(_localizer["I permessi di questo ruolo non possono essere modificati"]);
        }

        if (_currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            // Remove Root Permissions if the role is not created for Root Tenant
            request.Permissions.RemoveAll(u => u.StartsWith("Permissions.Root."));
        }

        var claims = await _roleManager.GetClaimsAsync(role);

        // Remove permissions that were previously selected
        foreach (var claim in claims.Where(x => !request.Permissions.Any(y => y == x.Value)))
        {
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (!result.Succeeded)
            {
                throw new InternalServerException(_localizer["Modifica dei permessi non riuscita"]);
            }
        }

        // Add all permissions that were not previously selected
        foreach (string permission in request.Permissions.Where(x => !claims.Any(y => y.Value == x)))
        {
            if (!string.IsNullOrEmpty(permission))
            {
                _db.RoleClaims.Add(new RoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = AppClaims.Permission,
                    ClaimValue = permission,
                    CreatedBy = _currentUser.GetUserId().ToString()
                });
                await _db.SaveChangesAsync(token);
            }
        }

        await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name!, true));
    }
}