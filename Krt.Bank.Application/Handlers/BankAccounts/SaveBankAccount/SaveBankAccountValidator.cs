using FluentValidation;
using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Interfaces.Repositories;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Application.Handlers.BankAccounts.SaveBankAccount
{
    public class SaveBankAccountValidator : AbstractValidator<BankAccountRequest>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserRepository _userRepository;

        public SaveBankAccountValidator(
            IBankAccountRepository bankAccountRepository,
            IUserRepository userRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId é obrigatório.")
                .MustAsync(UserExists)
                .WithMessage("Usuário não encontrado.");
        }

        private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(UserId.Create(userId));
            return user != null;
        }
    }
}
