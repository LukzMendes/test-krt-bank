using Krt.Bank.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krt.Bank.Infrastructure.Io
{
    public static class DbContextBootstrapper
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<KrtBankDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("KrtBankDbContext"));
            });

            return services;
        }
    }
}
