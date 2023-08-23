namespace Squares.Infrastructure.SecurityHeaders;

public class SecurityHeaders
{
    public string? XFrameOptions { get; set; }
    public string? XContentTypeOptions { get; set; }
    public string? ReferrerPolicy { get; set; }
    public string? PermissionsPolicy { get; set; }
    public string? SameSite { get; set; }
    public string? XXSSProtection { get; set; }

    // public List<string> ContentPolicy { get; set; }
}