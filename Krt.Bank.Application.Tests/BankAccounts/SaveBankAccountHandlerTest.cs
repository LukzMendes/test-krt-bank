using Krt.Bank.Application.Handlers.BankAccounts.CreateBankAccountHandler;
using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Handlers.BankAccounts.SaveBankAccount;
using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Users;
using Moq;
using System.Reflection;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Application.Tests.Handlers.BankAccounts
{
    public class SaveBankAccountHandlerTests
    {
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly SaveBankAccountValidator _validator;
        private readonly SaveBankAccountHandler _handler;

        public SaveBankAccountHandlerTests()
        {
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            // Criando o validador com os mocks
            _validator = new SaveBankAccountValidator(_bankAccountRepositoryMock.Object, _userRepositoryMock.Object);

            // Criando o handler com os mocks
            _handler = new SaveBankAccountHandler(
                _bankAccountRepositoryMock.Object,
                _validator,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateBankAccount_WhenValidRequest()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var accountNumber = "123456";

            var request = new BankAccountRequest
            {
                Id = null,
                AccountNumber = accountNumber,
                UserId = userId
            };

            var user = User.Create("João da Silva", "12345678900");

            // Força o ID via reflexão

            var idProperty = typeof(User)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(p => p.Name == "Id" && p.PropertyType == typeof(UserId));

            idProperty?.SetValue(user, UserId.Create(userId));


            var createdAccount = BankAccount.Create(accountNumber, user.Id, Money.Zero);
            createdAccount.User = user;

            _userRepositoryMock
                .Setup(x => x.GetAsync(It.Is<UserId>(id => id.Value == userId)))
                .ReturnsAsync(user);

            _bankAccountRepositoryMock
                .Setup(x => x.ExistsByNumberAsync(accountNumber))
                .ReturnsAsync(false);

            _bankAccountRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<BankAccount>()))
                .ReturnsAsync(createdAccount);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountNumber, result.AccountNumber);
            Assert.Equal(userId, result.Holder.Id);
            Assert.Equal(user.Name, result.Holder.HolderName);
            Assert.Equal(user.CPF, result.Holder.CPF);
        }
    }
}