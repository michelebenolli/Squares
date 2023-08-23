using System.Collections.ObjectModel;

namespace Squares.Shared.Authorization;
public static class AppAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Copy = nameof(Copy);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string Sort = nameof(Sort);
}

public static class AppResource
{
    public const string Tenants = nameof(Tenants);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
}

public static class AppPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new(AppAction.View, AppResource.Hangfire),
        new(AppAction.View, AppResource.Users),
        new(AppAction.Search, AppResource.Users),
        new(AppAction.Create, AppResource.Users),
        new(AppAction.Update, AppResource.Users),
        new(AppAction.Delete, AppResource.Users),
        new(AppAction.Export, AppResource.Users),
        new(AppAction.Sort, AppResource.Users),
        new(AppAction.View, AppResource.UserRoles),
        new(AppAction.Update, AppResource.UserRoles),
        new(AppAction.Search, AppResource.Roles),
        new(AppAction.View, AppResource.Roles),
        new(AppAction.Create, AppResource.Roles),
        new(AppAction.Update, AppResource.Roles),
        new(AppAction.Delete, AppResource.Roles),
        new(AppAction.View, AppResource.RoleClaims),
        new(AppAction.Update, AppResource.RoleClaims),
        new(AppAction.View, AppResource.Tenants, IsRoot: true),
        new(AppAction.Create, AppResource.Tenants, IsRoot: true),
        new(AppAction.Update, AppResource.Tenants, IsRoot: true),
    };

    public static IReadOnlyList<AppPermission> All { get; } = new ReadOnlyCollection<AppPermission>(_all);
    public static IReadOnlyList<AppPermission> Root { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<AppPermission> Admin { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<AppPermission> Basic { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record AppPermission(string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => GetName(Action, Resource);
    public static string GetName(string action, string resource) =>
        $"Permissions.{resource}.{action}";
}