using Krt.Bank.Application.Common.Pagination;
using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Interfaces.Repositories;
using static Krt.Bank.Domain.Users.User;

namespace Krt.Bank.Application.Handlers.BankAccounts.GetBankAccountHandler
{
    public class GetBankAccountsHandler(
        IBankAccountRepository bankAccountRepository)
    {
        public async Task<PaginatedResponse<BankAccountResponse>> Handle(GetBankAccountsRequest request,
            CancellationToken cancellationToken)
        {
            var paginated = await bankAccountRepository.GetPaginated(
                userId: request.UserId.HasValue ? UserId.Create(request.UserId.Value) : null,
                requestCreatedAtStart: request.CreatedAtStart,
                requestCreatedAtEnd: request.CreatedAtEnd,
                cpf: request.CPF,
                accountNumber: request.AccountNumber,
                toPaginated: request.ToPaginated());

            var items = paginated.Items
                .Select(x => x.ToResponse())
                .ToList();

            return paginated.ToPaginatedResponse(items);
        }
    }
}
