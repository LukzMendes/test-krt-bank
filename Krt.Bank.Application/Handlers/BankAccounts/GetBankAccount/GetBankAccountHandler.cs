using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Domain.BankAccounts;

namespace Krt.Bank.Application.Handlers.BankAccounts.GetBankAccount
{
    public class GetBankAccountHandler
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public GetBankAccountHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccountResponse> Handle(GetBankAccountRequest request, CancellationToken cancellationToken)
        {
            var bankAccountId = BankAccountId.Create(request.Id);
            var bankAccount = await _bankAccountRepository.GetAsync(bankAccountId);

            return bankAccount == null
                ? throw new KeyNotFoundException($"Conta bancária com ID {request.Id} não encontrada.") :
                bankAccount.ToResponse();
        }
    }

    public class GetBankAccountRequest
    {
        public Guid Id { get; set; }
    }
}
