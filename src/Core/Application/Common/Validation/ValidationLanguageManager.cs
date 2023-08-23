using FluentValidation.Resources;

namespace Squares.Application.Common.Validation;

public class ValidationLanguageManager : LanguageManager
{
    public ValidationLanguageManager()
    {
        AddTranslation("it", "EmailValidator", "Il valore non è un indirizzo email valido.");
        AddTranslation("it", "EqualValidator", "Il valore dovrebbe essere uguale a '{ComparisonValue}'.");
        AddTranslation("it", "ExactLengthValidator", "Il valore deve essere lungo {MaxLength} caratteri. Hai inserito {TotalLength} caratteri.");
        AddTranslation("it", "ExclusiveBetweenValidator", "Il valore deve essere compreso tra {From} e {To} (esclusi). Hai inserito {PropertyValue}.");
        AddTranslation("it", "GreaterThanOrEqualValidator", "Il valore deve essere maggiore o uguale a '{ComparisonValue}'.");
        AddTranslation("it", "GreaterThanValidator", "Il valore deve essere maggiore di '{ComparisonValue}'.");
        AddTranslation("it", "InclusiveBetweenValidator", "Il valore deve essere compreso tra {From} e {To}. Hai inserito {PropertyValue}.");
        AddTranslation("it", "LengthValidator", "Il valore deve essere lungo tra i {MinLength} e {MaxLength} caratteri. Hai inserito {TotalLength} caratteri.");
        AddTranslation("it", "MinimumLengthValidator", "Il valore deve essere lungo minimo {MinLength} caratteri. Hai inserito {TotalLength} caratteri.");
        AddTranslation("it", "MaximumLengthValidator", "Il valore deve essere lungo massimo {MaxLength} caratteri. Hai inserito {TotalLength} caratteri.");
        AddTranslation("it", "LessThanOrEqualValidator", "Il valore deve essere minore o uguale a '{ComparisonValue}'.");
        AddTranslation("it", "LessThanValidator", "Il valore deve essere minore di '{ComparisonValue}'.");
        AddTranslation("it", "NotEmptyValidator", "Il campo è obbligatorio.");
        AddTranslation("it", "NotEqualValidator", "Il valore non può essere uguale a '{ComparisonValue}'.");
        AddTranslation("it", "NotNullValidator", "Il campo è obbligatorio.");
        AddTranslation("it", "PredicateValidator", "Il valore inserito non è valido.");
        AddTranslation("it", "AsyncPredicateValidator", "Il valore inserito non è valido.");
        AddTranslation("it", "RegularExpressionValidator", "Il valore non è nel formato corretto.");
        AddTranslation("it", "CreditCardValidator", "Il valore non è un numero di carta di credito valido.");
        AddTranslation("it", "EmptyValidator", "Il campo dovrebbe essere vuoto.");
        AddTranslation("it", "NullValidator", "Il campo dovrebbe essere vuoto.");
        AddTranslation("it", "EnumValidator", "Il campo ha un intervallo di valori che non include '{PropertyValue}'.");
    }
}