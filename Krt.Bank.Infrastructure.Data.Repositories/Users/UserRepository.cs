using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Repositories;
using Krt.Bank.Domain.Users;
using Krt.Bank.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Krt.Bank.Infrastructure.Data.Repositories.Users
{
    public class UserRepository(KrtBankDbContext context, ILogger<IRepository<User>> logger)
    : Repository<User>(context, logger), IUserRepository
    {
        public async Task<Paginated<User>> GetPaginated(
            DateTimeOffset? requestCreatedAtStart,
            DateTimeOffset? requestCreatedAtEnd,
            string? name,
            string? cpf,
            Paginate toPaginated)
        {
            var query = context.Set<User>().AsNoTracking()
                .NotRemoved()
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.RemovedAt == null);

            if (requestCreatedAtStart.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= requestCreatedAtStart);
            }

            if (requestCreatedAtEnd.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= requestCreatedAtEnd);
            }

            if (name != null)
            {
                query = query.Where(x => x.Name == name);
            }

            if (cpf != null)
            {
                query = query.Where(x => x.CPF == cpf);
            }

            return await query.PaginateAsync(paginate: toPaginated);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await context.Set<User>().AsNoTracking()
                .NotRemoved()
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.RemovedAt == null)
                .ToListAsync();
        }

        public async Task<User?> GetByCPF(string cpf)
        {
            return await context.Set<User>()
                .AsNoTracking()
                .NotRemoved()
                .Where(x => x.CPF == cpf)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
