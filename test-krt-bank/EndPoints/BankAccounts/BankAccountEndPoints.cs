using Krt.Bank.Application.Common.Pagination;
using Krt.Bank.Application.Handlers.BankAccounts.CreateBankAccountHandler;
using Krt.Bank.Application.Handlers.BankAccounts.DisableBankAccount;
using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccount;
using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccountHandler;
using Microsoft.AspNetCore.Mvc;

namespace test_krt_bank.EndPoints.BankAccounts
{
    public static class BankAccountEndpoints
    {
        public static void MapBankAccountEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/bank-account")
                .WithTags("Bank Accounts")
                .WithDescription("Endpoints to manage bank accounts");

            group.MapPost("/", CreateBankAccountAsync)
                .WithName("CreateBankAccount")
                .WithSummary("Create a new bank account")
                .Produces<BankAccountResponse>(StatusCodes.Status201Created);

            group.MapGet("/{id:guid}", GetBankAccountAsync)
                .WithName("GetBankAccount")
                .WithSummary("Get a account")
                .Produces<BankAccountResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/all", GetBankAccountsAsync)
                .WithName("GetBankAccounts")
                .WithSummary("Get all bank accounts")
                .Produces<PaginatedResponse<BankAccountResponse>>(StatusCodes.Status200OK);

            group.MapPut("/{id:guid}", UpdateBankAccountAsync)
                .WithName("UpdateBankAccount")
                .WithSummary("Update an existing bank account")
                .Produces<BankAccountResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/", DisableBankAccountsAync)
                .WithName("DisableBankAccount")
                .WithSummary("Disable multiple bank accounts by IDs")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }

        private static Task DisableBankAccountsAync(
        [FromBody] DeleteBankAccountsRequest request,
        [FromServices] DisableBankAccountsHandler handler)
        {
            return handler.Handle(request);
        }

        private static async Task<BankAccountResponse> UpdateBankAccountAsync(
            [FromRoute] Guid id,
            [FromBody] BankAccountRequest request,
            [FromServices] SaveBankAccountHandler handler,
            CancellationToken cancellationToken)
        {
            request.Id = id;
            return await handler.Handle(request, cancellationToken);
        }


        private static Task<PaginatedResponse<BankAccountResponse>> GetBankAccountsAsync(
            [FromQuery] DateTimeOffset? createdAtStart,
            [FromQuery] DateTimeOffset? createdAtEnd,
            [FromQuery] Guid? UserId,
            [FromQuery] string? accountNumber,
            [FromQuery] string? cpf,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            [FromQuery] bool? asc,
            [FromQuery] string[] sortBy,
            [FromServices] GetBankAccountsHandler handler,
            CancellationToken cancellationToken)
        {
            var request = new GetBankAccountsRequest
            {
                CreatedAtEnd = createdAtEnd, 
                CreatedAtStart = createdAtStart,
                UserId = UserId,
                AccountNumber = accountNumber,
                CPF = cpf,
                Page = page,
                PageSize = pageSize,
                Ascending = asc,
                OrderBy = sortBy
            };

            return handler.Handle(request, cancellationToken);
        }

        private static Task<BankAccountResponse> GetBankAccountAsync(
            [FromRoute] Guid id,
            [FromServices] GetBankAccountHandler handler, 
            CancellationToken cancellationToken)
        {
            var request = new GetBankAccountRequest
            {
                Id = id
            };

            return handler.Handle(request, cancellationToken);
        }

        private static Task<BankAccountResponse> CreateBankAccountAsync(
            [FromBody] BankAccountRequest request,
            [FromServices] SaveBankAccountHandler handler,
            CancellationToken cancellationToken)
        {
            return handler.Handle(request, cancellationToken);
        }
    }
}
