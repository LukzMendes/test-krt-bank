using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Infrastructure.Data.Configuration.Mapping.BankAccounts
{
    public class BankAccountMapping : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("BankAccounts", "BankAccount");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(x => x.Value, x => BankAccountId.Create(x)).IsRequired();
            builder.Property(x => x.UserId).HasConversion(x => x.Value, x => UserId.Create(x)).IsRequired();
            builder.Property(x => x.AccountNumber).IsRequired();
            builder.Property(x => x.RemovedBy).IsRequired(false);
            builder.OwnsOne(x => x.Balance, balance =>
            {
                balance.Property(b => b.Amount)
                    .IsRequired();
            });
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
