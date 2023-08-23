using Squares.Application.Identity.Users.Requests;

namespace Squares.Application.Identity.Users.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator(
        IUserService userService,
        IStringLocalizer<UpdateUserValidator> localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (request, x, _) => !await userService.ExistsWithEmailAsync(x, request.Id))
                .WithMessage(localizer["Esiste già un utente con questa email"]);

        RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (request, x, _) => !await userService.ExistsWithPhoneNumberAsync(x!, request.Id))
                .WithMessage(localizer["Esiste già un utente con questo telefono"])
                .Unless(x => string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}