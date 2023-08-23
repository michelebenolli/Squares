namespace Squares.Application.Identity.Roles.Requests;
public class UpdateRoleRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}