using Squares.Application.Identity.Roles.Requests;

namespace Squares.Application.Identity.Roles.Validators;
public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator(
        IRoleService roleService,
        IStringLocalizer<UpdateRoleRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(async (request, x, _) => !await roleService.ExistsAsync(x, request.Id))
                .WithMessage(localizer["Esiste già un ruolo con questo nome"]);
    }
}