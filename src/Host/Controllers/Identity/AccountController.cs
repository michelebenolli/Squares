using Squares.Application.Identity.Account.Requests;
using Squares.Application.Identity.Users;
using Squares.Application.Identity.Users.Requests;

namespace Squares.Host.Controllers.Identity;

public class AccountController : VersionNeutralApiController
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("profile")]
    [OpenApiOperation("Get profile of the current user", "")]
    public async Task<ActionResult<ApplicationUserDto>> GetProfileAsync()
    {
        return int.TryParse(User?.GetUserId(), out int userId)
            ? Ok(await Mediator.Send(new GetUserRequest(userId)))
            : Unauthorized();
    }

    [HttpPut("profile")]
    [OpenApiOperation("Update profile of the current user", "")]
    public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
    {
        _ = int.TryParse(User?.GetUserId(), out int userId);
        if (userId != request.Id)
        {
            return Unauthorized();
        }

        await Mediator.Send(request);
        return Ok();
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request a password reset email for a user", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return _userService.ForgotPasswordAsync(request, GetOrigin());
    }

    [HttpPost("reconfirm-email")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Reconfirm email address for a user", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public Task ReconfirmEmailAsync(ConfirmEmailRequest request)
    {
        return _userService.ReconfirmEmailAsync(request, GetOrigin());
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [OpenApiOperation("Reset the user password", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        return _userService.ResetPasswordAsync(request);
    }

    [HttpGet("permissions")]
    [OpenApiOperation("Get permissions of the current user", "")]
    public async Task<ActionResult<List<string>>> GetPermissionsAsync()
    {
        return int.TryParse(User?.GetUserId(), out int userId)
            ? Ok(await Mediator.Send(new GetPermissionsRequest(userId)))
            : Unauthorized();
    }

    [HttpPut("change-password")]
    [OpenApiOperation("Change password of the current user", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        if (!int.TryParse(User?.GetUserId(), out int userId))
        {
            return Unauthorized();
        }

        await _userService.ChangePasswordAsync(request, userId);
        return Ok();
    }

    private string GetOrigin() => "http://localhost:4200"; // TODO: => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}