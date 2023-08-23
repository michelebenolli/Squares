using Squares.Application.Common.Exceptions;
using Squares.Application.Identity.Tokens;
using Squares.Application.Identity.Tokens.Requests;
using Squares.Application.Identity.Users;
using Squares.Infrastructure.Auth;
using Squares.Infrastructure.Auth.Jwt;
using Squares.Infrastructure.Multitenancy;
using Squares.Shared.Authorization;
using Squares.Shared.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Squares.Infrastructure.Identity;

internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<TokenService> _localizer;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    private readonly AppTenant? _currentTenant;
    private readonly IUserService _userService;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> localizer,
        AppTenant? currentTenant,
        IOptions<SecuritySettings> securitySettings,
        IUserService userService)
    {
        _userManager = userManager;
        _localizer = localizer;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
        _userService = userService;
        _securitySettings = securitySettings.Value;
    }

    public async Task<TokenDto> GetTokenAsync(TokenRequest request, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(_currentTenant?.Id)
            || await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException(_localizer["Autenticazione non riuscita"]);
        }

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            throw new UnauthorizedException(_localizer["Email non confermata"]);
        }

        if (_currentTenant.Id != MultitenancyConstants.Root.Id && (!_currentTenant.IsActive ||
           (_currentTenant.EndDate != null && DateTime.UtcNow > _currentTenant.EndDate)))
        {
            throw new UnauthorizedException(_localizer["Il tenant non è attivo"]);
        }

        var permissions = await _userService.GetPermissionsAsync(user.Id, token);
        if (!user.IsActive || permissions?.Any() != true)
        {
            throw new UnauthorizedException(_localizer["L'utente non è attivo"]);
        }

        user = await _userManager.Users
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == user.Id, token);

        return await GenerateTokensAndUpdateUser(user!, permissions);
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal.GetEmail();

        // TODO: Check here if the tenant is null (it is given in JWT, no?)
        // ERROR HERE, null (because tenant not set in request header and JWT not given?)
        var user = await _userManager.FindByEmailAsync(userEmail!);
        if (user is null)
        {
            throw new UnauthorizedException(_localizer["Autenticazione non riuscita"]);
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException(_localizer["Refresh token non valido"]);
        }

        user = await _userManager.Users.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == user.Id);
        var permissions = await _userService.GetPermissionsAsync(user!.Id);

        return await GenerateTokensAndUpdateUser(user!, permissions);
    }

    private async Task<TokenDto> GenerateTokensAndUpdateUser(ApplicationUser user, List<string> permissions)
    {
        string token = GenerateJwt(user);
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
        await _userManager.UpdateAsync(user);

        return new TokenDto
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
            Permissions = permissions
        };
    }

    private string GenerateJwt(ApplicationUser user)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user));
    }

    private IEnumerable<Claim> GetClaims(ApplicationUser user)
    {
        return new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.User.FirstName),
            new(ClaimTypes.Surname, user.User.LastName),
            new(AppClaims.Tenant, _currentTenant!.Id),
            new(AppClaims.ImageUrl, user.User?.Image ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };
    }

    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        return securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)
            ? throw new UnauthorizedException(_localizer["Token non valido"])
            : principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}