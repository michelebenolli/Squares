using Squares.Application.Multitenancy.Requests;

namespace Squares.Application.Multitenancy.Validators;

public class ToggleTenantValidator : AbstractValidator<ToggleTenantRequest>
{
    public ToggleTenantValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty();
    }
}