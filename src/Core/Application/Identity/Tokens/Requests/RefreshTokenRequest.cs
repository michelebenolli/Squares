namespace Squares.Application.Identity.Tokens.Requests;

public record RefreshTokenRequest(string Token, string RefreshToken);