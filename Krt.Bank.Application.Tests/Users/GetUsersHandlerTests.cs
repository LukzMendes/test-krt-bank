namespace Krt.Bank.Application.Tests.Users
{
    using Krt.Bank.Application.Handlers.Users.GetUsersHandler;
    using Krt.Bank.Application.Interfaces.Repositories;
    using Krt.Bank.Domain.Users;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;
    using static Krt.Bank.Domain.Users.User;

    public class GetUsersHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUsersHandler _handler;

        public GetUsersHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUsersHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfUserResponses_WhenUsersExist()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userIdVO = UserId.Create(userId);

            var user = User.Create("João da Silva", "12345678900");
            typeof(User)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(p => p.Name == "Id" && p.PropertyType == typeof(UserId))
                .SetValue(user, userIdVO);

            var users = new List<User> { user };

            _userRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _handler.Handle();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(userId, result.First().Id);
            Assert.Equal(user.Name, result.First().HolderName);
            Assert.Equal(user.CPF, result.First().CPF);
        }
    }

}
