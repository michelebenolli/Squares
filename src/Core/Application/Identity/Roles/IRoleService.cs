using Squares.Application.Identity.Roles.Requests;

namespace Squares.Application.Identity.Roles;

public interface IRoleService : ITransientService
{
    Task<IPagedList<RoleDto>> SearchAsync(PagedRequest request, CancellationToken token);
    Task<RoleDto> GetByIdAsync(int id);
    Task<List<RoleDto>> ListAsync(CancellationToken token);
    Task<int> CreateAsync(CreateRoleRequest request);
    Task UpdateAsync(UpdateRoleRequest request);
    Task DeleteAsync(int id);
    Task<int> GetCountAsync(CancellationToken token);
    Task<bool> ExistsAsync(string roleName, int? excludeId = null);
    List<string> GetPermissions();
    Task<List<string>> GetPermissionsAsync(int roleId);
    Task UpdatePermissionsAsync(UpdatePermissionsRequest request, CancellationToken token);
}