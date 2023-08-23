using Squares.Application.Identity.Roles;

namespace Squares.Application.Identity.Users;
public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Image { get; set; }

    public List<RoleDto>? Roles { get; set; }
}