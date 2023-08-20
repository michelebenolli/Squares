using Squares.Shared.Events;

namespace Squares.Domain.Common.Contracts;
public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}