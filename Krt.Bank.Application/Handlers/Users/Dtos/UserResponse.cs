using Krt.Bank.Domain.Users;

namespace Krt.Bank.Application.Handlers.Users.Dtos
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public required string HolderName { get; set; }
        public required string CPF { get; set; }

        public static UserResponse ToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id.Value,
                HolderName = user.Name,
                CPF = user.CPF
            };
        }
    }
}
