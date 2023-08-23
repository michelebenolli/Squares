namespace Squares.Application.Identity.Users;
public class ApplicationUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Image { get; set; }
    public bool IsActive { get; set; }
}