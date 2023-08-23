using Squares.Application.Identity.Roles.Requests;

namespace Squares.Application.Identity.Roles.Validators;
public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator(
        IRoleService roleService,
        IStringLocalizer<CreateRoleRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(async (x, _) => !await roleService.ExistsAsync(x))
                .WithMessage(localizer["Esiste già un ruolo con questo nome"]);
    }
}