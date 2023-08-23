namespace Squares.Domain.Identity;

public class User : AuditableEntity
{
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Image { get; set; }
}