using Squares.Shared.Events;

namespace Squares.Application.Common.Events;
public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}