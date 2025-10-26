namespace Krt.Bank.Events.BankAccounts
{
    public class BankAccountCreatedEvent : IntegrationEvent
    {
        public Guid BankAccountId { get; }
        public Guid UserId { get; }
        public string AccountNumber { get; }
        public DateTime CreatedAt { get; }

        public BankAccountCreatedEvent(Guid bankAccountId, Guid userId, string accountNumber, DateTime createdAt)
        {
            BankAccountId = bankAccountId;
            UserId = userId;
            AccountNumber = accountNumber;
            CreatedAt = createdAt;
        }
    }
}
