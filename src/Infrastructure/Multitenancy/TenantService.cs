using AutoMapper;
using Finbuckle.MultiTenant;
using Squares.Application.Common.Exceptions;
using Squares.Application.Common.Persistence;
using Squares.Application.Multitenancy;
using Squares.Infrastructure.Persistence;
using Squares.Infrastructure.Persistence.Initialization;
using Squares.Shared.Multitenancy;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Squares.Application.Multitenancy.Requests;

namespace Squares.Infrastructure.Multitenancy;

internal class TenantService : ITenantService
{
    private readonly IMultiTenantStore<AppTenant> _tenantStore;
    private readonly IConnectionStringSecurer _csSecurer;
    private readonly IDatabaseInitializer _dbInitializer;
    private readonly IStringLocalizer<TenantService> _localizer;
    private readonly IMapper _mapper;
    private readonly DatabaseSettings _dbSettings;

    public TenantService(
        IMultiTenantStore<AppTenant> tenantStore,
        IConnectionStringSecurer csSecurer,
        IDatabaseInitializer dbInitializer,
        IStringLocalizer<TenantService> localizer,
        IOptions<DatabaseSettings> dbSettings,
        IMapper mapper)
    {
        _tenantStore = tenantStore;
        _csSecurer = csSecurer;
        _dbInitializer = dbInitializer;
        _localizer = localizer;
        _mapper = mapper;
        _dbSettings = dbSettings.Value;
    }

    public async Task<List<TenantDto>> ListAsync()
    {
        var tenants = _mapper.Map<List<TenantDto>>(await _tenantStore.GetAllAsync());
        tenants.ForEach(x => x.ConnectionString = _csSecurer.MakeSecure(x.ConnectionString));
        return tenants;
    }

    public async Task<TenantDto> GetByIdAsync(string id)
    {
        var tenant = await GetTenantInfoAsync(id);
        return _mapper.Map<TenantDto>(tenant);
    }

    public async Task<string> GetByDomainAsync(string domain)
    {
        var tenants = await _tenantStore.GetAllAsync();
        var tenant = tenants?.FirstOrDefault(x => x.Domain == domain);
        return tenant?.Id ?? MultitenancyConstants.Root.Id;
    }

    public async Task<string> CreateAsync(CreateTenantRequest request, CancellationToken token)
    {
        try
        {
            if (request.ConnectionString?.Trim() == _dbSettings.ConnectionString.Trim())
            {
                request.ConnectionString = string.Empty;
            }

            var tenant = _mapper.Map<AppTenant>(request);
            await _tenantStore.TryAddAsync(tenant);
            await _dbInitializer.InitializeApplicationDbForTenantAsync(tenant, token);
        }
        catch
        {
            await _tenantStore.TryRemoveAsync(request.Id);
            throw;
        }

        return request.Id;
    }

    public async Task UpdateAsync(UpdateTenantRequest request)
    {
        var tenant = await GetTenantInfoAsync(request.Id);
        _mapper.Map(request, tenant);

        await _tenantStore.TryUpdateAsync(tenant);
    }

    public async Task ToggleAsync(string id)
    {
        if (id == MultitenancyConstants.Root.Id)
        {
            throw new ConflictException(_localizer["Si è verificato un errore"]);
        }

        var tenant = await GetTenantInfoAsync(id);
        tenant.IsActive = !tenant.IsActive;
        await _tenantStore.TryUpdateAsync(tenant);
    }

    public async Task<bool> ExistsWithIdAsync(string id)
    {
        return await _tenantStore.TryGetAsync(id) is not null;
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        var tenants = await _tenantStore.GetAllAsync();
        return tenants.Any(x => x.Name == name);
    }

    private async Task<AppTenant> GetTenantInfoAsync(string id)
    {
        return await _tenantStore.TryGetAsync(id) ??
            throw new NotFoundException(_localizer["Tenant non trovato"]);
    }
}