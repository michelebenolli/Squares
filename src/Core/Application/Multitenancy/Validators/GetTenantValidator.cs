using Squares.Application.Multitenancy.Requests;

namespace Squares.Application.Multitenancy.Validators;

public class GetTenantValidator : AbstractValidator<GetTenantRequest>
{
    public GetTenantValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty();
    }
}