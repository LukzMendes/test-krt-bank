using Krt.Bank.Domain.Common;

namespace Krt.Bank.Domain.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity);
        Task<T?> GetAsync(Id id);
        Task<T> RemoveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T?> GetTrackingAsync(Id id);
    }
}
