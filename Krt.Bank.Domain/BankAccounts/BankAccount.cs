using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Users;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Domain.BankAccounts
{
    public class BankAccount : Entity
    {
        public BankAccountId Id { get; private set; }
        public string AccountNumber { get; private set; }
        public Guid? RemovedBy { get; private set; }
        public Money Balance { get; private set; }

        public UserId UserId { get; private set; }
        public virtual User User { get; set; }

        private BankAccount() { }

        private BankAccount(BankAccountId id, string accountNumber, UserId userId, Money initialBalance)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id), "O identificador da conta não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("O número da conta não pode estar vazio.", nameof(accountNumber));
            AccountNumber = accountNumber;

            UserId = userId ?? throw new ArgumentNullException(nameof(userId), "O identificador do usuário não pode ser nulo.");
            Balance = initialBalance ?? throw new ArgumentNullException(nameof(initialBalance), "O saldo inicial não pode ser nulo.");

            if (initialBalance.Amount < 0)
                throw new ArgumentException("O saldo inicial não pode ser negativo.", nameof(initialBalance));
        }

        public static BankAccount Create(string accountNumber, UserId userId, Money initialBalance)
        {
            return new BankAccount(BankAccountId.Create(Guid.NewGuid()), accountNumber, userId, initialBalance);
        }

        public void UpdateAccountNumber(string newAccountNumber)
        {
            if (string.IsNullOrWhiteSpace(newAccountNumber))
                throw new ArgumentException("O novo número da conta não pode estar vazio.", nameof(newAccountNumber));

            AccountNumber = newAccountNumber;
            Update(); 
        }

        public void RemoveAccount(Guid removedBy)
        {
            RemovedBy = removedBy;
            Remove(); 
        }

        public void Deposit(decimal amount)
        {
            if ( amount <= 0)
                throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(amount));

            Balance = Money.Create(amount);
            Update();
        }

        public void ChangeUser(UserId newUserId)
        {
            UserId = newUserId ?? 
                throw new ArgumentNullException(nameof(newUserId), "O identificador do novo usuário não pode ser nulo.");

            Update();
        }
    }

    public class BankAccountId : Id
    {
        public BankAccountId(Guid value) : base(value)
        {
        }

        public static BankAccountId Create(Guid value)
        {
            return new BankAccountId(value);
        }
    }
}
