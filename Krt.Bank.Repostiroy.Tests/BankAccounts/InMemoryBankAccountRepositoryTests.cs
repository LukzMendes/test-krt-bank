namespace Krt.Bank.Repostiroy.Tests.BankAccounts
{
    using Krt.Bank.Domain.BankAccounts;
    using Krt.Bank.Domain.Common;
    using Krt.Bank.Infrastructure.Data.Repositories.BankAccounts;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using static Krt.Bank.Domain.Users.User;

    public class InMemoryBankAccountRepositoryTests
    {
        private readonly InMemoryBankAccountRepository _repository;

        public InMemoryBankAccountRepositoryTests()
        {
            _repository = new InMemoryBankAccountRepository();
        }

        [Fact]
        public async Task AddAsync_ShouldStoreBankAccount()
        {
            // Arrange
            var userId = UserId.Create(Guid.NewGuid());
            var account = BankAccount.Create("123456", userId, Money.Zero);

            // Act
            var result = await _repository.AddAsync(account);

            // Assert
            Assert.Equal(account.Id, result.Id);
            var stored = await _repository.GetAsync(account.Id);
            Assert.NotNull(stored);
            Assert.Equal("123456", stored.AccountNumber);
        }

        [Fact]
        public async Task ExistsByNumberAsync_ShouldReturnTrue_WhenAccountExists()
        {
            // Arrange
            var userId = UserId.Create(Guid.NewGuid());
            var account = BankAccount.Create("999999", userId, Money.Zero);
            await _repository.AddAsync(account);

            // Act
            var exists = await _repository.ExistsByNumberAsync("999999");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task GetByIdAndUser_ShouldReturnAccount_WhenExists()
        {
            // Arrange
            var userId = UserId.Create(Guid.NewGuid());
            var account = BankAccount.Create("ABC123", userId, Money.Zero);
            await _repository.AddAsync(account);

            // Act
            var result = await _repository.GetByIdAndUser(userId, account.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(account.Id, result.Id);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteAccount()
        {
            // Arrange
            var userId = UserId.Create(Guid.NewGuid());
            var account = BankAccount.Create("DEL123", userId, Money.Zero);
            await _repository.AddAsync(account);

            // Act
            await _repository.RemoveAsync(account);
            var result = await _repository.GetAsync(account.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceAccount()
        {
            // Arrange
            var userId = UserId.Create(Guid.NewGuid());
            var account = BankAccount.Create("UPD123", userId, Money.Zero);
            await _repository.AddAsync(account);

            account.UpdateAccountNumber("NEW123");
            var updated = await _repository.UpdateAsync(account);

            // Act
            var result = await _repository.GetAsync(account.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NEW123", result.AccountNumber);
        }
    }
}
