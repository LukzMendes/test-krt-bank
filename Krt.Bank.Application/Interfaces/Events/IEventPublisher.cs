using Krt.Bank.Domain.Events;

namespace Krt.Bank.Application.Interfaces.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
