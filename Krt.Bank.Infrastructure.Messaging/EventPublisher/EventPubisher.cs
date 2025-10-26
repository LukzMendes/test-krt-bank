using Krt.Bank.Application.Interfaces.Events;
using Krt.Bank.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Krt.Bank.Infrastructure.Messaging.EventPublisher
{
    public class ConsoleEventPublisher : IEventPublisher
    {
        private readonly ILogger<ConsoleEventPublisher> _logger;

        public ConsoleEventPublisher(ILogger<ConsoleEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Evento publicado: {EventType} - {@Event}", @event.GetType().Name, @event);
            return Task.CompletedTask;
        }
    }
}
