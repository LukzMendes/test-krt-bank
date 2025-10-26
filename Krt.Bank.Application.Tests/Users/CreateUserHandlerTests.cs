namespace Krt.Bank.Application.Tests.Users
{
    using FluentValidation;
    using Krt.Bank.Application.Handlers.Users.CreateUserHandler;
    using Krt.Bank.Application.Interfaces.Repositories;
    using Krt.Bank.Domain.Users;
    using Moq;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static Krt.Bank.Domain.Users.User;

    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CreateUserValidator _validator;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new CreateUserValidator(); // se não tiver dependências
            _handler = new CreateUserHandler(_validator, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenValidRequest()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var request = new CreateUserRequest
            {
                Name = "João da Silva",
                CPF = "123.456.789-00"
            };

            var expectedCpf = new string(request.CPF.Where(char.IsDigit).ToArray());

            var user = User.Create(request.Name, expectedCpf);
            typeof(User)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(p => p.Name == "Id" && p.PropertyType == typeof(UserId))
                .SetValue(user, UserId.Create(userId));

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(request.Name, result.HolderName);
            Assert.Equal(expectedCpf, result.CPF);
        }
    }
}
