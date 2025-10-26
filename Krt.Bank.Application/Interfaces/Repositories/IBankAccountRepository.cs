using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Repositories;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        void DeleteAsync(BankAccount bankAccount);
        Task<BankAccount> GetByIdAndUser(UserId userId, BankAccountId bankAccountId);
        Task<bool> ExistsByNumberAsync(string accountNumber);
        Task<Paginated<BankAccount>> GetPaginated(UserId userId, 
            DateTimeOffset? requestCreatedAtStart, 
            DateTimeOffset? requestCreatedAtEnd, 
            string? cpf, 
            string? accountNumber, 
            Paginate toPaginated);
        Task<BankAccount?> GetAsync(Id id);
    }
}

