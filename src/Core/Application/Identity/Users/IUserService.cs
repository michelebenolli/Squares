using Squares.Application.Identity.Account.Requests;
using Squares.Application.Identity.Roles;
using Squares.Application.Identity.Users.Requests;
using System.Security.Claims;

namespace Squares.Application.Identity.Users;
public interface IUserService : ITransientService
{
    Task<ApplicationUserDto?> GetByIdAsync(int userId, CancellationToken token);
    Task<List<ApplicationUserDto>?> ListAsync(CancellationToken token);
    Task<IPagedList<ApplicationUserDto>> SearchAsync(SearchUserRequest request, CancellationToken token);
    Task<int> CreateAsync(CreateUserRequest request);
    Task<int> RegisterAsync(RegisterUserRequest request);
    Task UpdateAsync(UpdateUserRequest request, CancellationToken token);
    Task DeleteAsync(int userId);
    Task<int> GetCountAsync(CancellationToken token);
    Task<int> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<List<string>> GetPermissionsAsync(int userId);
    Task<List<RoleDto>> GetRolesAsync(int userId, CancellationToken token);
    Task<List<string>> GetPermissionsAsync(int userId, CancellationToken token);
    Task<bool> HasPermissionAsync(int userId, string permission, CancellationToken token = default);
    Task InvalidatePermissionCacheAsync(int userId, CancellationToken token);
    Task ToggleAsync(int userId, bool active, CancellationToken token);
    Task<bool> ExistsWithEmailAsync(string email, int? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, int? exceptId = null);
    Task ConfirmEmailAsync(int userId, string code, CancellationToken token);
    Task ReconfirmEmailAsync(ConfirmEmailRequest request);
    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request, int userId);
}