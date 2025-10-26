using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Handlers.BankAccounts.SaveBankAccount;
using Krt.Bank.Events.BankAccounts;
using Microsoft.AspNetCore.Mvc;

namespace test_krt_bank.EndPoints.BankAccounts
{
    public static class BankAccountEventsEndPoints
    {
        public static void MapBankAccountEventsEndpoints(this IEndpointRouteBuilder builder)
        {

            var eventGroup = builder.MapGroup("api/events/bank-account")
                .WithTags("Bank Account Events")
                .WithDescription("Endpoints to simulate bank account events");

            eventGroup.MapPost("/", async (
                [FromBody] BankAccountRequest request,
                [FromServices] BankAccountCreatedEventHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(request, cancellationToken);
                return Results.Accepted();
            });

        }
    }
}
