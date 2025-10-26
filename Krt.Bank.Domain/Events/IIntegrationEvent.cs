namespace Krt.Bank.Domain.Events
{
    public interface IIntegrationEvent
    {
        Guid EventId { get; }
        DateTime OccurredAt { get; }
    }
}
