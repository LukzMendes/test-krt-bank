namespace Krt.Bank.Application.Tests.BankAccounts
{
    using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccount;
    using Krt.Bank.Application.Interfaces.Repositories;
    using Krt.Bank.Domain.BankAccounts;
    using Krt.Bank.Domain.Common;
    using Krt.Bank.Domain.Users;
    using Moq;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static Krt.Bank.Domain.Users.User;

    public class GetBankAccountHandlerTests
    {
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly GetBankAccountHandler _handler;

        public GetBankAccountHandlerTests()
        {
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _handler = new GetBankAccountHandler(_bankAccountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnBankAccountResponse_WhenAccountExists()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userIdVO = UserId.Create(userId);
            var accountIdVO = BankAccountId.Create(accountId);

            var user = User.Create("João da Silva", "12345678900");
            typeof(User)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(p => p.Name == "Id" && p.PropertyType == typeof(UserId))
                .SetValue(user, userIdVO);

            var bankAccount = BankAccount.Create("123456", userIdVO, Money.Zero);
            typeof(BankAccount)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(p => p.Name == "Id" && p.PropertyType == typeof(BankAccountId))
                .SetValue(bankAccount, accountIdVO);
            bankAccount.User = user;

            _bankAccountRepositoryMock
                .Setup(x => x.GetAsync(accountIdVO))
                .ReturnsAsync(bankAccount);

            var request = new GetBankAccountRequest { Id = accountId };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.AccountNumber);
            Assert.Equal(user.Name, result.Holder.HolderName);
            Assert.Equal(user.CPF, result.Holder.CPF);
            Assert.Equal(userId, result.Holder.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var accountIdVO = BankAccountId.Create(accountId);

            _bankAccountRepositoryMock
                .Setup(x => x.GetAsync(accountIdVO))
                .ReturnsAsync((BankAccount?)null);

            var request = new GetBankAccountRequest { Id = accountId };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal($"Conta bancária com ID {request.Id} não encontrada.", exception.Message);
        }
    }
}
