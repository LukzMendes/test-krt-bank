using Krt.Bank.Application.Common.Pagination;
using Krt.Bank.Application.Handlers.BankAccounts.CreateBankAccountHandler;
using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Handlers.Users.CreateUserHandler;
using Krt.Bank.Application.Handlers.Users.Dtos;
using Krt.Bank.Application.Handlers.Users.GetUsersHandler;
using Microsoft.AspNetCore.Mvc;

namespace test_krt_bank.EndPoints.User
{
    public static class UserEndPoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/user")
                .WithTags("Users")
                .WithDescription("Endpoints to manage users");

            group.MapPost("/", CreateUserAsync)
                .WithName("CreateUser")
                .WithSummary("Create a new user")
                .Produces<BankAccountResponse>(StatusCodes.Status201Created);

            group.MapGet("/all", GetUsersAsync)
                .WithName("GetUsers")
                .WithSummary("Get all users")
                .Produces<PaginatedResponse<UserResponse>>(StatusCodes.Status200OK);
        }

        private static async Task<List<UserResponse>> GetUsersAsync([FromServices] GetUsersHandler handler)
        {
            return await handler.Handle();
        }

        private static Task<UserResponse> CreateUserAsync(
            [FromBody] CreateUserRequest request,
            [FromServices] CreateUserHandler handler,
            CancellationToken cancellationToken)
        {
            return handler.Handle(request, cancellationToken);
        }
    }
}
