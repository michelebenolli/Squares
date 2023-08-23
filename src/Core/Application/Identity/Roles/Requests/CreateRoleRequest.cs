namespace Squares.Application.Identity.Roles.Requests;
public class CreateRoleRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}