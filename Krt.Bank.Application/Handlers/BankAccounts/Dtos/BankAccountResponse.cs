using Krt.Bank.Application.Handlers.Users.Dtos;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common;

namespace Krt.Bank.Application.Handlers.BankAccounts.Dtos
{
    public class BankAccountResponse
    {
        public Guid Id { get; set; }
        public string? AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public Money? Balance { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public UserResponse? Holder { get; set; }
    }

    public static class BankAccountResponseExtensions
    {
        public static BankAccountResponse ToResponse(this BankAccount bankAccount)
        {
            return new BankAccountResponse
            {
                Id = bankAccount.Id.Value,
                AccountNumber = bankAccount.AccountNumber,
                IsActive = bankAccount.IsActive,
                Balance = bankAccount.Balance,
                CreatedAt = bankAccount.CreatedAt,
                Holder = new UserResponse
                {
                    Id = bankAccount.User.Id.Value,
                    HolderName = bankAccount.User.Name,
                    CPF = bankAccount.User.CPF
                }
            };
        }
    }
}
