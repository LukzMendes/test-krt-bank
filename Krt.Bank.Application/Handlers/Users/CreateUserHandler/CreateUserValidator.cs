namespace Krt.Bank.Application.Handlers.Users.CreateUserHandler
{
    using FluentValidation;
    using Krt.Bank.Application.Interfaces.Repositories;

    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.CPF)
                .NotEmpty()
                .WithMessage("CPF é obrigatório.")
                .MustAsync(async (cpf, cancellation) =>
                {
                    var existingUser = await userRepository.GetByCPF(cpf);
                    return existingUser == null;
                })
                .WithMessage("Já existe um usuário com este CPF.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome é obrigatório.");
        }
    }
}
