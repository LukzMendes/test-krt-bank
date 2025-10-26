using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;
using Krt.Bank.Domain.Exceptions;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Application.Handlers.BankAccounts.DisableBankAccount
{
    public class DisableBankAccountsHandler(IBankAccountRepository bankAccountRepository,
        IUserRepository userRepository)
    {
        public async Task Handle(DeleteBankAccountsRequest request)
        {
            //await unitOfWork.BeginBlockTransactionAsync();

            foreach (var id in request.Ids)
            {
                var userId = UserId.Create(request.UserId);
                var user = await userRepository.GetAsync(userId) ??
                    throw new NotFoundException($"Usuario com ID {id} não encontrado.");

                var bankAccountId = BankAccountId.Create(id);
                var bankAccount = await bankAccountRepository.GetByIdAndUser(userId, bankAccountId);

                bankAccount.RemoveAccount(user.Id.Value);
            }

            //await unitOfWork.CommitTransactionAsync();
        }
    }

    public class DeleteBankAccountsRequest
    {
        public Guid UserId { get; set; }
        public Guid[] Ids { get; set; } = [];
    }
}
