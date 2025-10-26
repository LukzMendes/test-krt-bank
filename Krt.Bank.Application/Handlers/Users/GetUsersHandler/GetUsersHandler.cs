using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Handlers.Users.Dtos;
using Krt.Bank.Application.Interfaces.Repositories;

namespace Krt.Bank.Application.Handlers.Users.GetUsersHandler
{
    public class GetUsersHandler(
        IUserRepository userRepository)
    {
        public async Task<List<UserResponse>> Handle()
        {
            var users = await userRepository.GetAllAsync();
            return users.Select(UserResponse.ToResponse).ToList();
        }
    }
}

