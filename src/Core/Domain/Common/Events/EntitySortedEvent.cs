namespace Squares.Domain.Common.Events;

public static class EntitySortedEvent
{
    public static EntitySortedEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IEntity
        => new(entity);
}

public class EntitySortedEvent<TEntity> : DomainEvent
    where TEntity : IEntity
{
    internal EntitySortedEvent(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; }
}