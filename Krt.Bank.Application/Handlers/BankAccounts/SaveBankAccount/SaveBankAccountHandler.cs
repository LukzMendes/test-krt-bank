using FluentValidation;
using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Handlers.BankAccounts.SaveBankAccount;
using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Common;
using Krt.Bank.Domain.Exceptions;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Application.Handlers.BankAccounts.CreateBankAccountHandler
{
    public class SaveBankAccountHandler(
        IBankAccountRepository bankAccountRepository,
        SaveBankAccountValidator validator,
        IUserRepository userRepository)
    {
        public async Task<BankAccountResponse> Handle(BankAccountRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            if (request.Id.HasValue)
            {
                return await Update(request);
            }

            return await Add(request);
        }

        private async Task<BankAccountResponse> Add(BankAccountRequest request)
        {
            if (string.IsNullOrEmpty(request.AccountNumber))
            {
                throw new DomainException("Número da conta bancária é obrigatório para criação.");
            }

            if (await bankAccountRepository.ExistsByNumberAsync(request.AccountNumber))
            {
                throw new DomainException($"Número da conta bancária {request.AccountNumber} já existe.");
            }

            var userId = UserId.Create(request.UserId);
            var user = await userRepository.GetAsync(userId) ??
                throw new NotFoundException($"Usuário com ID {request.UserId} não encontrado.");

            var bankAccount = BankAccount.Create(
                accountNumber: request.AccountNumber,
                userId: userId,
                Money.Zero);

            bankAccount.User = user;

            var bankAccountAdded = await bankAccountRepository.AddAsync(bankAccount);

            return bankAccountAdded.ToResponse();
        }

        private async Task<BankAccountResponse> Update(BankAccountRequest request)
        {
            var banckAccountId = BankAccountId.Create(request.Id.Value);
            var bankAccount = await bankAccountRepository.GetAsync(banckAccountId) ??
                throw new NotFoundException($"Conta bancária com ID {request.Id} não encontrada.");


            if (request.AccountNumber != null && bankAccount.AccountNumber != request.AccountNumber)
            {
                bankAccount.UpdateAccountNumber(request.AccountNumber);
            }

            if (request.UserId != Guid.Empty && bankAccount.UserId.Value != request.UserId)
            {
                var newUserId = UserId.Create(request.UserId);
                var user = await userRepository.GetAsync(newUserId) ??
                    throw new NotFoundException($"Usuário com ID {request.UserId} não encontrado.");
                bankAccount.ChangeUser(newUserId);
            }

            var updatedBankAccount = await bankAccountRepository.UpdateAsync(bankAccount);

            return updatedBankAccount.ToResponse();
        }
    }
}
