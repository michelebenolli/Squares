using Microsoft.AspNetCore.Identity;
using Squares.Domain.Identity;

namespace Squares.Infrastructure.Identity;
public class ApplicationUser : IdentityUser<int>
{
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? ObjectId { get; set; }

    [PersonalData]
    public User User { get; set; } = default!;
}