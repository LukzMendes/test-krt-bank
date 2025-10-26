namespace Krt.Bank.Application.Tests.BankAccounts
{
    using Krt.Bank.Application.Handlers.BankAccounts.DisableBankAccount;
    using Krt.Bank.Application.Interfaces.Repositories;
    using Krt.Bank.Domain.BankAccounts;
    using Krt.Bank.Domain.Common;
    using Krt.Bank.Domain.Users;
    using Moq;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;
    using static Krt.Bank.Domain.Users.User;

    public class DisableBankAccountsHandlerTests
    {
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DisableBankAccountsHandler _handler;

        public DisableBankAccountsHandlerTests()
        {
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new DisableBankAccountsHandler(
                _bankAccountRepositoryMock.Object,
                _userRepositoryMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldRemoveAccounts_WhenValidRequest()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var accountIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

            var request = new DeleteBankAccountsRequest
            {
                UserId = userId,
                Ids = accountIds
            };

            var user = User.Create("João da Silva", "12345678900");
            typeof(User)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(p => p.Name == "Id" && p.PropertyType == typeof(UserId))
                .SetValue(user, UserId.Create(userId));

            _userRepositoryMock
                .Setup(x => x.GetAsync(It.Is<UserId>(id => id.Value == userId)))
                .ReturnsAsync(user);

            foreach (var accId in accountIds)
            {
                var bankAccount = BankAccount.Create("ACC-" + accId.ToString("N").Substring(0, 6), UserId.Create(userId), Money.Zero);
                bankAccount.User = user;

                _bankAccountRepositoryMock
                    .Setup(x => x.GetByIdAndUser(UserId.Create(userId), BankAccountId.Create(accId)))
                    .ReturnsAsync(bankAccount);
            }

            _unitOfWorkMock.Setup(x => x.BeginBlockTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(request);

            // Assert
            _unitOfWorkMock.Verify(x => x.BeginBlockTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);

            foreach (var accId in accountIds)
            {
                _bankAccountRepositoryMock.Verify(x =>
                    x.GetByIdAndUser(UserId.Create(userId), BankAccountId.Create(accId)), Times.Once);
            }
        }
    }
}
