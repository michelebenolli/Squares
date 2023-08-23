using Squares.Application.Multitenancy.Requests;

namespace Squares.Application.Multitenancy.Validators;

public class CreateTenantValidator : AbstractValidator<CreateTenantRequest>
{
    public CreateTenantValidator(
        ITenantService tenantService,
        IStringLocalizer<CreateTenantValidator> localizer,
        IConnectionStringValidator connectionStringValidator)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (id, _) => !await tenantService.ExistsWithIdAsync(id))
            .WithMessage(localizer["Esiste già un tenant con questo id"]);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(async (name, _) => !await tenantService.ExistsWithNameAsync(name!))
            .WithMessage(localizer["Esiste già un tenant con questo nome"]);

        RuleFor(x => x.ConnectionString)
            .Must((_, cs) => string.IsNullOrWhiteSpace(cs) || connectionStringValidator.TryValidate(cs))
            .WithMessage(localizer["La stringa di connessione non è valida"]);

        RuleFor(x => x.AdminEmail)
            .NotEmpty()
            .EmailAddress();
    }
}