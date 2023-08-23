using Squares.Application.Identity.Account.Requests;

namespace GPL.Application.Identity.Account.Validators;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty();
    }
}