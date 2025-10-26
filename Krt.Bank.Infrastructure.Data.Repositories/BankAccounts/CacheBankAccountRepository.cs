using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Common.Pagination;
using Microsoft.Extensions.Caching.Memory;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Infrastructure.Data.Repositories.BankAccounts
{
    public class CachedBankAccountRepository : IBankAccountRepository
    {
        private readonly IBankAccountRepository _innerRepository;
        private readonly IMemoryCache _cache;
        public CachedBankAccountRepository(IBankAccountRepository innerRepository, IMemoryCache cache)
        {
            _innerRepository = innerRepository;
            _cache = cache;
        }

        public async Task<BankAccount?> GetAsync(Id id)
        {
            var cacheKey = $"BankAccount:{id.Value}:{DateTime.UtcNow:yyyyMMdd}";

            if (_cache.TryGetValue(cacheKey, out BankAccount cachedAccount))
            {
                return cachedAccount;
            }

            var account = await _innerRepository.GetAsync(id);

            if (account != null)
            {
                _cache.Set(cacheKey, account, TimeSpan.FromDays(1));
            }

            return account;
        }

        public Task<BankAccount?> GetTrackingAsync(Id id)
            => _innerRepository.GetTrackingAsync(id);

        public Task<BankAccount> AddAsync(BankAccount entity)
            => _innerRepository.AddAsync(entity);


        public async Task<BankAccount> UpdateAsync(BankAccount entity)
        {
            var result = await _innerRepository.UpdateAsync(entity);

            var cacheKey = $"BankAccount:{entity.Id.Value}:{DateTime.UtcNow:yyyyMMdd}";
            _cache.Set(cacheKey, result, TimeSpan.FromDays(1)); // Atualiza o cache com os dados novos

            return result;
        }


        public async Task<BankAccount> RemoveAsync(BankAccount entity)
        {
            var result = await _innerRepository.RemoveAsync(entity);

            var cacheKey = $"BankAccount:{entity.Id.Value}:{DateTime.UtcNow:yyyyMMdd}";
            _cache.Remove(cacheKey); // Remove do cache

            return result;
        }

        public Task<Paginated<BankAccount>> GetPaginated(
            UserId userId,
            DateTimeOffset? requestCreatedAtStart,
            DateTimeOffset? requestCreatedAtEnd,
            string? cpf,
            string? accountNumber,
            Paginate toPaginated)
            => _innerRepository.GetPaginated(userId, requestCreatedAtStart, requestCreatedAtEnd, cpf, accountNumber, toPaginated);

        public void DeleteAsync(BankAccount bankAccount)
            => _innerRepository.DeleteAsync(bankAccount);

        public Task<BankAccount> GetByIdAndUser(UserId userId, BankAccountId bankAccountId)
            => _innerRepository.GetByIdAndUser(userId, bankAccountId);

        public Task<bool> ExistsByNumberAsync(string accountNumber)
            => _innerRepository.ExistsByNumberAsync(accountNumber);
    }
}
