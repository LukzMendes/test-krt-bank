using Krt.Bank.Application.Handlers.BankAccounts.CreateBankAccountHandler;
using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccountHandler;
using Krt.Bank.Application.Handlers.Users.CreateUserHandler;
using Krt.Bank.Application.Handlers.Users.GetUsersHandler;
using Microsoft.Extensions.DependencyInjection;

namespace Krt.Bank.Infrastructure.Io
{
    public static class ApplicationBootstrapper
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            //Users
            services.AddScoped<CreateUserHandler>();
            services.AddScoped<GetUsersHandler>();

            //BankAccounts
            services.AddScoped<SaveBankAccountHandler>();
            services.AddScoped<GetBankAccountsHandler>();

            //Documents

            return services;
        }
    }
}
