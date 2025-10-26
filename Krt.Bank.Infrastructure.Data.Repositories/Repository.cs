using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Repositories;
using Krt.Bank.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Krt.Bank.Infrastructure.Data.Repositories
{
    public class Repository<T>(KrtBankDbContext context, ILogger<IRepository<T>> logger)
        : IRepository<T>
        where T : Entity
    {
        public async Task<T> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            entity.Update();
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> GetAsync(Id id)
        {
            var result = await context.Set<T>().AsNoTracking().NotRemoved().FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<T?> GetTrackingAsync(Id id)
        {
            var result = await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            entity.Remove();
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
