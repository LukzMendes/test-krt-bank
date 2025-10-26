using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krt.Bank.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task BeginBlockTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
