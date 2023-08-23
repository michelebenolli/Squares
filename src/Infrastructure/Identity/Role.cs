using Microsoft.AspNetCore.Identity;

namespace Squares.Infrastructure.Identity;
public class Role : IdentityRole<int>
{
    public string? Description { get; set; }

    public Role(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}