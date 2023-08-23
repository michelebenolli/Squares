namespace Squares.Application.Identity.Roles.Requests;
public class UpdatePermissionsRequest
{
    public int RoleId { get; set; }
    public List<string> Permissions { get; set; } = default!;
}