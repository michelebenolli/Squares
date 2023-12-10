using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using X.PagedList;
using Squares.Application.Common.Caching;
using Squares.Application.Common.Events;
using Squares.Application.Common.Exceptions;
using Squares.Application.Common.Interfaces;
using Squares.Application.Common.Mailing;
using Squares.Application.Common.Persistence;
using Squares.Application.Identity.Users.Requests;
using Squares.Application.Identity.Users;
using Squares.Application.Multitenancy;
using Squares.Domain.Identity;
using Squares.Infrastructure.Auth;
using Squares.Infrastructure.Identity.Users;
using Squares.Infrastructure.Persistence.Context;
using Squares.Shared.Authorization;
using Squares.Application.Common.Models;

namespace Squares.Infrastructure.Identity;
internal partial class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly DatabaseContext _db;
    private readonly IStringLocalizer<UserService> _localizer;
    private readonly IJobService _jobService;
    private readonly IMailService _mailService;
    private readonly SecuritySettings _securitySettings;
    private readonly IEmailTemplateService _templateService;
    private readonly IEventPublisher _events;
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;
    private readonly ITenantInfo _currentTenant;
    private readonly IMapper _mapper;
    private readonly IRepository<Role> _roles;
    private readonly ITenantService _tenantService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<Role> roleManager,
        DatabaseContext db,
        IStringLocalizer<UserService> localizer,
        IJobService jobService,
        IMailService mailService,
        IEmailTemplateService templateService,
        IEventPublisher events,
        ICacheService cache,
        ICacheKeyService cacheKeys,
        ITenantInfo currentTenant,
        IOptions<SecuritySettings> securitySettings,
        IMapper mapper,
        IRepository<Role> roles,
        ITenantService tenantService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
        _localizer = localizer;
        _jobService = jobService;
        _mailService = mailService;
        _templateService = templateService;
        _events = events;
        _cache = cache;
        _cacheKeys = cacheKeys;
        _currentTenant = currentTenant;
        _mapper = mapper;
        _securitySettings = securitySettings.Value;
        _roles = roles;
        _tenantService = tenantService;
    }

    public async Task<ApplicationUserDto?> GetByIdAsync(int userId, CancellationToken token)
    {
        var user = await _userManager.Users
            .Include(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, token);

        return _mapper.Map<ApplicationUserDto?>(user);
    }

    public async Task<List<ApplicationUserDto>?> ListAsync(CancellationToken token)
    {
        var users = await _userManager.Users
            .Include(x => x.User)
            .AsNoTracking().ToListAsync(token);

        return _mapper.Map<List<ApplicationUserDto>>(users);
    }

    public async Task<IPagedList<ApplicationUserDto>> SearchAsync(SearchUserRequest request, CancellationToken token)
    {
        var query = _userManager.Users.WithSpecification(new SearchUserSpec(request));

        int? roleId = request.GetFilter<int?>("@roleId");
        if (roleId != null)
        {
            var roles = await _roles.ListAsync(token);
            string? roleName = roles?.Find(x => x.Id == roleId)?.Name;
            var roleUsers = await _userManager.Users.WithSpecification(new SearchUserSpec(request)).ToList()
                    .Where(x => _userManager.GetRolesAsync(x).Result.ToList()
                        .Any(x => x.ToLower() == roleName?.ToLower()))
                    .ToPagedListAsync(request.PageNumber, request.PageSize, token);

            return _mapper.Map<IPagedList<ApplicationUserDto>>(roleUsers);
        }

        var users = await query.ToPagedListAsync(request.PageNumber, request.PageSize, token);
        return _mapper.Map<IPagedList<ApplicationUserDto>>(users);
    }

    public Task<int> GetCountAsync(CancellationToken token)
    {
        return _userManager.Users.AsNoTracking().CountAsync(token);
    }

    public async Task ToggleAsync(int userId, bool active, CancellationToken token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, token);
        _ = user ?? throw new NotFoundException(_localizer["Utente non trovato"]);

        if (await _userManager.IsInRoleAsync(user, AppRoles.Admin))
        {
            throw new ConflictException(_localizer["Un amministratore non può essere disattivato"]);
        }

        user.IsActive = active;
        await _userManager.UpdateAsync(user);
        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
    }

    public async Task<bool> ExistsWithEmailAsync(string email, int? exceptId = null)
    {
        EnsureValidTenant();
        return await _userManager.FindByEmailAsync(email) is ApplicationUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, int? exceptId = null)
    {
        EnsureValidTenant();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _userManager.Users.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == userId);
        _ = user ?? throw new NotFoundException(_localizer["Utente non trovato"]);

        if (await _userManager.IsInRoleAsync(user, AppRoles.Admin))
        {
            throw new ConflictException(_localizer["Un amministratore non può essere eliminato"]);
        }

        using var transaction = _db.Database.BeginTransaction();
        _db.Users.Remove(user.User);

        if (_db.SaveChanges() > 0)
        {
            transaction.Commit();
            await _events.PublishAsync(new ApplicationUserDeletedEvent(user.Id));
        }
        else
        {
            transaction.Rollback();
            throw new Exception(_localizer["Cancellazione non riuscita"]);
        }
    }

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(_currentTenant?.Id))
        {
            throw new UnauthorizedException(_localizer["Tenant non valido"]);
        }
    }

    private async Task<TenantDto> GetTenant()
    {
        EnsureValidTenant();
        return await _tenantService.GetByIdAsync(_currentTenant.Id!);
    }
}