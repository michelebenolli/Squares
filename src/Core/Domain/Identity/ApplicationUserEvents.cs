namespace Squares.Domain.Identity;

public abstract class ApplicationUserEvent : DomainEvent
{
    public int UserId { get; set; }
    protected ApplicationUserEvent(int userId)
    {
        UserId = userId;
    }
}

public class ApplicationUserCreatedEvent : ApplicationUserEvent
{
    public ApplicationUserCreatedEvent(int userId)
        : base(userId)
    {
    }
}

public class ApplicationUserUpdatedEvent : ApplicationUserEvent
{
    public bool RolesUpdated { get; set; }

    public ApplicationUserUpdatedEvent(int userId, bool rolesUpdated = false)
        : base(userId)
    {
        RolesUpdated = rolesUpdated;
    }
}

public class ApplicationUserDeletedEvent : ApplicationUserEvent
{
    public ApplicationUserDeletedEvent(int userId)
        : base(userId)
    {
    }
}