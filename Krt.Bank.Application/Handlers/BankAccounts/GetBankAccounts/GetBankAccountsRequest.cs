using Krt.Bank.Application.Common.Pagination;

namespace Krt.Bank.Application.Handlers.BankAccounts.GetBankAccountHandler
{
    public class GetBankAccountsRequest : PaginatedRequest
    {
        public Guid? UserId { get; set; }
        public DateTimeOffset? CreatedAtStart { get; set; }
        public DateTimeOffset? CreatedAtEnd { get; set; }
        public string? CPF { get; set; }
        public string? AccountNumber { get; set; }
    }
}
