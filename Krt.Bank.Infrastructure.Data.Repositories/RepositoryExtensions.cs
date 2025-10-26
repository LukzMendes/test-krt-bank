using Krt.Bank.Domain.Common;

namespace Krt.Bank.Infrastructure.Data.Repositories
{
    public static class RepositoryExtensions
    {
        public static IQueryable<T> NotRemoved<T>(this IQueryable<T> queryable) where T : Entity?
        {
            return queryable.Where(x => x.RemovedAt == null);
        }
    }
}
