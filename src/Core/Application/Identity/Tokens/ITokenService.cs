using Squares.Application.Identity.Tokens.Requests;

namespace Squares.Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<TokenDto> GetTokenAsync(TokenRequest request, CancellationToken token);
    Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request);
}