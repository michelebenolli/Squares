using Squares.Application.Identity.Tokens.Requests;

namespace Squares.Application.Identity.Tokens.Validators;
public class TokenValidator : AbstractValidator<TokenRequest>
{
    public TokenValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}