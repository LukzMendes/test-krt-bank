using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Krt.Bank.Infrastructure.Data.Repositories
{
    public class UnitOfWork(KrtBankDbContext context) : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task BeginBlockTransactionAsync()
        {
            await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        public async Task CommitTransactionAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
