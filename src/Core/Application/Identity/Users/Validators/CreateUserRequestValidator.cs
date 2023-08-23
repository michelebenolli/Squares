using Squares.Application.Identity.Users.Requests;

namespace Squares.Application.Identity.Users.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator(
        IUserService userService,
        IStringLocalizer<CreateUserValidator> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (x, _) => !await userService.ExistsWithEmailAsync(x))
                .WithMessage(localizer["Esiste già un utente con questa email"]);

        RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (x, _) => !await userService.ExistsWithPhoneNumberAsync(x!))
                .WithMessage(localizer["Esiste già un utente con questo telefono"])
                .Unless(x => string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}