namespace Squares.Shared.Multitenancy;

public class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "michele.benolli@gmail.com";
    }

    public const string DefaultPassword = "Password123!";

    public const string TenantIdName = "tenant";
}