using System.Text.Json.Serialization;

namespace Krt.Bank.Application.Handlers.BankAccounts.Dtos
{
    public class BankAccountRequest
    {
        [JsonIgnore] public Guid? Id { get; set; }
        public string? AccountNumber { get; set; }
        public Guid UserId { get; set; }

    }
}
