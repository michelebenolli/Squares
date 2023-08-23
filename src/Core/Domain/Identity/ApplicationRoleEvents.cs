namespace Squares.Domain.Identity;

public abstract class ApplicationRoleEvent : DomainEvent
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = default!;

    protected ApplicationRoleEvent(int roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }
}

public class ApplicationRoleCreatedEvent : ApplicationRoleEvent
{
    public ApplicationRoleCreatedEvent(int roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class ApplicationRoleUpdatedEvent : ApplicationRoleEvent
{
    public bool PermissionsUpdated { get; set; }

    public ApplicationRoleUpdatedEvent(int roleId, string roleName, bool permissionsUpdated = false)
        : base(roleId, roleName)
    {
        PermissionsUpdated = permissionsUpdated;
    }
}

public class ApplicationRoleDeletedEvent : ApplicationRoleEvent
{
    public bool PermissionsUpdated { get; set; }

    public ApplicationRoleDeletedEvent(int roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}