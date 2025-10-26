using FluentValidation;
using Krt.Bank.Application.Handlers.Users.Dtos;
using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.Users;

namespace Krt.Bank.Application.Handlers.Users.CreateUserHandler
{
    public class CreateUserHandler(
            CreateUserValidator validator,
            IUserRepository userRepository)
    {
        public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var user = User.Create(
                name: request.Name,
                cpf: new string(request.CPF.Where(char.IsDigit).ToArray()));

            var userAdded = await userRepository.AddAsync(user);

            return UserResponse.ToResponse(userAdded);
        }
    }
}
