using Microsoft.AspNetCore.Identity;

namespace Squares.Infrastructure.Identity;
public class RoleClaim : IdentityRoleClaim<int>
{
    public string? CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
}