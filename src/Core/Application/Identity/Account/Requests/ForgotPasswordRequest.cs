namespace Squares.Application.Identity.Account.Requests;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = default!;
}