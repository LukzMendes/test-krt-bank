using Krt.Bank.Domain.Events;

namespace Krt.Bank.Events
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public Guid EventId { get; private set; }
        public DateTime OccurredAt { get; private set; }

        protected IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
        }
    }
}
