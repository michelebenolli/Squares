using Squares.Domain.Identity;
using Squares.Infrastructure.Identity;
using Squares.Infrastructure.Multitenancy;
using Squares.Infrastructure.Persistence.Context;
using Squares.Shared.Authorization;
using Squares.Shared.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Squares.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly AppTenant _currentTenant;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(AppTenant currentTenant, RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
    {
        _currentTenant = currentTenant;
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
    }

    public async Task SeedDatabaseAsync(DatabaseContext dbContext, CancellationToken token)
    {
        await SeedRolesAsync(dbContext);
        await SeedAdminUserAsync();
        await _seederRunner.RunSeedersAsync(token);
    }

    private async Task SeedRolesAsync(DatabaseContext dbContext)
    {
        foreach (string roleName in AppRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                is not Role role)
            {
                // Create the role
                _logger.LogInformation("Seeding {role} role for '{tenantId}' tenant.", roleName, _currentTenant.Id);
                role = new Role(roleName, $"{roleName} role for {_currentTenant.Id} tenant");
                await _roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == AppRoles.Basic)
            {
                await AssignPermissionsToRoleAsync(dbContext, AppPermissions.Basic, role);
            }
            else if (roleName == AppRoles.Admin)
            {
                await AssignPermissionsToRoleAsync(dbContext, AppPermissions.Admin, role);

                if (_currentTenant.Id == MultitenancyConstants.Root.Id)
                {
                    await AssignPermissionsToRoleAsync(dbContext, AppPermissions.Root, role);
                }
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(DatabaseContext dbContext, IReadOnlyList<AppPermission> permissions, Role role)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaims.Any(c => c.Type == AppClaims.Permission && c.Value == permission.Name))
            {
                dbContext.RoleClaims.Add(new RoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = AppClaims.Permission,
                    ClaimValue = permission.Name,
                    CreatedBy = "ApplicationDbSeeder"
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
        {
            return;
        }

        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
            is not ApplicationUser adminUser)
        {
            string adminUserName = $"{_currentTenant.Id.Trim()}.{AppRoles.Admin}".ToLowerInvariant();

            adminUser = new ApplicationUser
            {
                Email = _currentTenant.AdminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true,
                User = new User
                {
                    UserName = adminUserName,
                    FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
                    LastName = AppRoles.Admin,
                    Email = _currentTenant.AdminEmail
                }
            };

            _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, MultitenancyConstants.DefaultPassword);
            await _userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, AppRoles.Admin))
        {
            _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
            await _userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
        }
    }
}