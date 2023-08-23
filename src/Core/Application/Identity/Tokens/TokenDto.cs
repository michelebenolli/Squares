namespace Squares.Application.Identity.Tokens;
public class TokenDto
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime RefreshTokenExpiryTime { get; set; }
    public List<string>? Permissions { get; set; }
}