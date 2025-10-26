using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Repositories;
using Krt.Bank.Domain.Users;

namespace Krt.Bank.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByCPF(string cpf);
        Task<Paginated<User>> GetPaginated(DateTimeOffset? requestCreatedAtStart, DateTimeOffset? requestCreatedAtEnd, string? name, string? cpf, Paginate toPaginated);
    }
}
