using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Infrastructure.Data.Repositories;
using Krt.Bank.Infrastructure.Data.Repositories.BankAccounts;
using Krt.Bank.Infrastructure.Data.Repositories.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Krt.Bank.Infrastructure.Io
{
    public static class RepositoryBootstrapper
    {
        public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services)
        {
            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Users
            services.AddScoped<IUserRepository, UserRepository>();

            // bankAccounts
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            return services;

        }
    }
}
