namespace Squares.Domain.Common.Contracts;

public interface IEntity
{
    int Id { get; }
    List<DomainEvent> DomainEvents { get; }
}