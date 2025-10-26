using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Krt.Bank.Infrastructure.Data.Configuration
{
    public class KrtBankDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        public KrtBankDbContext(DbContextOptions<KrtBankDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(KrtBankDbContext).Assembly);
        }
    }
}
