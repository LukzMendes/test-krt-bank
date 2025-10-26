using Azure.Core;
using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Common.Pagination;
using Krt.Bank.Domain.Users;
using System.Globalization;

namespace Krt.Bank.Infrastructure.Data.Repositories.Users
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User> _users = new();

        public Task<User?> GetAsync(Id id)
        {
            _users.TryGetValue(id.Value, out var user);
            return Task.FromResult(user);
        }

        public Task<List<User>> GetAllAsync()
        {
            var users = _users.Values
                .Where(x => x.RemovedAt == null)
                .OrderBy(x => x.CreatedAt);

            return Task.FromResult(users.ToList());
        }

        public Task<User?> GetByCPF(string cpf)
        {
            var user = _users.Values
                .Where(x => x.CPF == new string(cpf.Where(char.IsDigit).ToArray()) && x.IsActive == true)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefault();

            return Task.FromResult(user);
        }

        public Task<User?> GetTrackingAsync(Id id)
        {
            return GetAsync(id);
        }

        public Task<User> AddAsync(User entity)
        {
            _users[entity.Id.Value] = entity;
            return Task.FromResult(entity);
        }

        public Task<User> UpdateAsync(User entity)
        {
            _users[entity.Id.Value] = entity;
            return Task.FromResult(entity);
        }

        public Task<User> RemoveAsync(User entity)
        {
            _users.Remove(entity.Id.Value);
            return Task.FromResult(entity);
        }

        public Task<Paginated<User>> GetPaginated(
            DateTimeOffset? requestCreatedAtStart,
            DateTimeOffset? requestCreatedAtEnd,
            string? name,
            string? cpf,
            Paginate toPaginated)
        {
            var query = _users.Values
                .Where(x => x.RemovedAt == null) 
                .OrderBy(x => x.CreatedAt)
                .AsEnumerable();

            if (requestCreatedAtStart.HasValue)
                query = query.Where(x => x.CreatedAt >= requestCreatedAtStart.Value);

            if (requestCreatedAtEnd.HasValue)
                query = query.Where(x => x.CreatedAt <= requestCreatedAtEnd.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name == name);

            if (!string.IsNullOrWhiteSpace(cpf))
                query = query.Where(x => x.CPF == cpf);

            var paginated = query.PaginateInMemory(toPaginated);
            return Task.FromResult(paginated);
        }
    }
}
