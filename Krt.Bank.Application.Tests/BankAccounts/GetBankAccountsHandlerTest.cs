namespace Krt.Bank.Application.Tests.BankAccounts
{
    using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccountHandler;
    using Krt.Bank.Application.Interfaces.Repositories;
    using Krt.Bank.Domain.BankAccounts;
    using Krt.Bank.Domain.Common;
    using Krt.Bank.Domain.Common.Pagination;
    using Krt.Bank.Domain.Users;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static Krt.Bank.Domain.Users.User;

    public class GetBankAccountsHandlerTests
    {
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly GetBankAccountsHandler _handler;

        public GetBankAccountsHandlerTests()
        {
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _handler = new GetBankAccountsHandler(_bankAccountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedBankAccounts_WhenValidRequest()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userIdVO = UserId.Create(userId);

            var request = new GetBankAccountsRequest
            {
                UserId = userId,
                CreatedAtStart = null,
                CreatedAtEnd = null,
                CPF = null,
                AccountNumber = null,
                Page = 1,
                PageSize = 10,
                OrderBy = new[] { "createdAt" },
                Ascending = true
            };

            var user = User.Create("João da Silva", "12345678900");
            typeof(User)
                .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                .First(p => p.Name == "Id" && p.PropertyType == typeof(UserId))
                .SetValue(user, userIdVO);

            var bankAccount = BankAccount.Create("123456", userIdVO, Money.Zero);
            bankAccount.User = user;

            var paginated = new Paginated<BankAccount>(
                items: new List<BankAccount> { bankAccount },
                total: 1,
                page: 1,
                pageSize: 10,
                orderBy: request.OrderBy,
                ascending: request.Ascending
            );

            _bankAccountRepositoryMock
                .Setup(x => x.GetPaginated(
                    It.Is<UserId?>(id => id == userIdVO),
                    request.CreatedAtStart,
                    request.CreatedAtEnd,
                    request.CPF,
                    request.AccountNumber,
                    It.Is<Paginate>(p => p.Page == request.Page && p.PageSize == request.PageSize)))
                .ReturnsAsync(paginated);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("123456", result.Items.First().AccountNumber);
            Assert.Equal(user.Name, result.Items.First().Holder.HolderName);
            Assert.Equal(user.CPF, result.Items.First().Holder.CPF);
            Assert.Equal(1, result.Page);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.Total);
            Assert.Equal(1, result.TotalPages);
        }
    }
}
