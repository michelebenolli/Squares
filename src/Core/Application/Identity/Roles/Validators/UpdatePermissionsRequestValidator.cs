using Squares.Application.Identity.Roles.Requests;

namespace Squares.Application.Identity.Roles.Validators;
public class UpdatePermissionsRequestValidator : AbstractValidator<UpdatePermissionsRequest>
{
    public UpdatePermissionsRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.Permissions)
            .NotNull();
    }
}