namespace Krt.Bank.Events.BankAccounts
{
    public class BankAccountUpdatedEvent : IntegrationEvent
    {
        public Guid BankAccountId { get; }
        public Guid UserId { get; }
        public string NewAccountNumber { get; }
        public DateTime UpdatedAt { get; }

        public BankAccountUpdatedEvent(Guid bankAccountId, Guid userId, string newAccountNumber, DateTime updatedAt)
        {
            BankAccountId = bankAccountId;
            UserId = userId;
            NewAccountNumber = newAccountNumber;
            UpdatedAt = updatedAt;
        }
    }
}
