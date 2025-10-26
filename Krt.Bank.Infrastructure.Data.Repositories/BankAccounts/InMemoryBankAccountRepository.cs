using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Exceptions;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Infrastructure.Data.Repositories.BankAccounts
{
    public class InMemoryBankAccountRepository : IBankAccountRepository
    {
        private readonly Dictionary<Guid, BankAccount> _bankAccounts = new();

        public Task<BankAccount> AddAsync(BankAccount entity)
        {
            _bankAccounts[entity.Id.Value] = entity;
            return Task.FromResult(entity);
        }

        public Task<BankAccount?> GetAsync(Id id)
        {
            _bankAccounts.TryGetValue(id.Value, out var account);
            return Task.FromResult(account);
        }

        public Task<BankAccount?> GetTrackingAsync(Id id)
        {
            return GetAsync(id);
        }

        public Task<BankAccount> RemoveAsync(BankAccount entity)
        {
            _bankAccounts.Remove(entity.Id.Value);
            return Task.FromResult(entity);
        }

        public Task<BankAccount> UpdateAsync(BankAccount entity)
        {
            _bankAccounts[entity.Id.Value] = entity;
            return Task.FromResult(entity);
        }


        public Task<Paginated<BankAccount>> GetPaginated(
            UserId userId,
            DateTimeOffset? requestCreatedAtStart,
            DateTimeOffset? requestCreatedAtEnd,
            string? cpf,
            string? accountNumber,
            Paginate toPaginated)
        {
            var query = _bankAccounts.Values.AsEnumerable();

            if (requestCreatedAtStart.HasValue)
                query = query.Where(x => x.CreatedAt >= requestCreatedAtStart.Value);

            if (requestCreatedAtEnd.HasValue)
                query = query.Where(x => x.CreatedAt <= requestCreatedAtEnd.Value);

            if (userId != null)
                query = query.Where(x => x.UserId == userId);

            if (!string.IsNullOrWhiteSpace(cpf))
                query = query.Where(x => x.User?.CPF == cpf);

            if (!string.IsNullOrWhiteSpace(accountNumber))
                query = query.Where(x => x.AccountNumber == accountNumber);

            var paginated = query.PaginateInMemory(toPaginated);
            return Task.FromResult(paginated);
        }


        public void DeleteAsync(BankAccount bankAccount)
        {
            _bankAccounts.Remove(bankAccount.Id.Value);
        }

        public Task<BankAccount> GetByIdAndUser(UserId userId, BankAccountId bankAccountId)
        {
            var bankAccount = _bankAccounts.Values
                .FirstOrDefault(x =>
                    x.RemovedAt == null &&
                    x.UserId == userId &&
                    x.Id == bankAccountId);

            return bankAccount is null
                ? throw new NotFoundException($"Conta bancária com ID {bankAccountId.Value} não encontrada para o usuário {userId.Value}.")
                : Task.FromResult(bankAccount);
        }

        public Task<bool> ExistsByNumberAsync(string accountNumber)
        {
            var exists = _bankAccounts.Values.Any(x =>
                x.AccountNumber == accountNumber);

            return Task.FromResult(exists);
        }

    }
}
