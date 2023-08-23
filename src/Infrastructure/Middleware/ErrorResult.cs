namespace Squares.Infrastructure.Middleware;

public class ErrorResult
{
    public int StatusCode { get; set; }
    public string? Exception { get; set; }
    public string? Source { get; set; }
    public string? ErrorId { get; set; }
    public List<string> Messages { get; set; } = new();
    public Dictionary<string, List<string>>? Errors { get; set; }
}