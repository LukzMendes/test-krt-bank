namespace Krt.Bank.Events.BankAccounts
{
    public class BankAccountDisabledEvent : IntegrationEvent
    {
        public Guid BankAccountId { get; }
        public Guid UserId { get; }
        public DateTime DisabledAt { get; }

        public BankAccountDisabledEvent(Guid bankAccountId, Guid userId, DateTime disabledAt)
        {
            BankAccountId = bankAccountId;
            UserId = userId;
            DisabledAt = disabledAt;
        }
    }
}
