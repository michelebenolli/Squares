using Squares.Application.Multitenancy.Requests;

namespace Squares.Application.Multitenancy;

public interface ITenantService
{
    Task<List<TenantDto>> ListAsync();
    Task<TenantDto> GetByIdAsync(string id);
    Task<string> CreateAsync(CreateTenantRequest request, CancellationToken token);
    Task UpdateAsync(UpdateTenantRequest request);
    Task ToggleAsync(string id);
    Task<bool> ExistsWithIdAsync(string id);
    Task<bool> ExistsWithNameAsync(string name);
    Task<string> GetByDomainAsync(string domain);
}