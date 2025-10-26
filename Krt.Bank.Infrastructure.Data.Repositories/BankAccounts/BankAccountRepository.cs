using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Exceptions;
using Krt.Bank.Domain.Repositories;
using Krt.Bank.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Infrastructure.Data.Repositories.BankAccounts
{
    public class BankAccountRepository(KrtBankDbContext context, ILogger<IRepository<BankAccount>> logger)
    : Repository<BankAccount>(context, logger), IBankAccountRepository
    {
        private readonly KrtBankDbContext _context = context;

        public async Task<Paginated<BankAccount>> GetPaginated(UserId userId,
            DateTimeOffset? requestCreatedAtStart,
            DateTimeOffset? requestCreatedAtEnd,
            string? cpf,
            string? accountNumber,
            Paginate toPaginated)
        {
            var query = _context.Set<BankAccount>().AsNoTracking()
                .Include(x => x.User)
                .AsSplitQuery()
                .OrderBy(x => x.CreatedAt)
                .NotRemoved();

            if (requestCreatedAtStart.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= requestCreatedAtStart);
            }

            if (requestCreatedAtEnd.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= requestCreatedAtEnd);
            }

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (cpf != null)
            {
                query = query.Where(x => x.User.CPF == cpf);
            }

            if (accountNumber != null)
            {
                query = query.Where(x => x.AccountNumber == accountNumber);
            }


            return await query.PaginateAsync(paginate: toPaginated);
        }

        public void DeleteAsync(BankAccount bankAccount)
        {
            _context.Set<BankAccount>().Remove(bankAccount);
            _context.SaveChanges();
        }

        public async Task<BankAccount> GetByIdAndUser(UserId userId, BankAccountId bankAccountId)
        {
            var bankAccount = await GetAsync(bankAccountId);

            if (bankAccount == null || bankAccount.UserId != userId)
            {
                throw new NotFoundException($"Conta bancária com ID {bankAccountId.Value} não encontrada para o usuário {userId.Value}.");
            }

            return bankAccount;
        }

        public async Task<bool> ExistsByNumberAsync(string accountNumber)
        {
            return await _context.Set<BankAccount>()
                .AsNoTracking()
                .AnyAsync(x => x.AccountNumber == accountNumber);
        }
    }
}
