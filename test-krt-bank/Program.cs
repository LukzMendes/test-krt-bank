//using Krt.Bank.Infrastructure.Io;

using Krt.Bank.Application.Handlers.BankAccounts.CreateBankAccountHandler;
using Krt.Bank.Application.Handlers.BankAccounts.DisableBankAccount;
using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccount;
using Krt.Bank.Application.Handlers.BankAccounts.GetBankAccountHandler;
using Krt.Bank.Application.Handlers.BankAccounts.SaveBankAccount;
using Krt.Bank.Application.Handlers.Users.CreateUserHandler;
using Krt.Bank.Application.Handlers.Users.GetUsersHandler;
using Krt.Bank.Application.Interfaces.Events;
using Krt.Bank.Application.Interfaces.Repositories;
using Krt.Bank.Infrastructure.Data.Repositories;
using Krt.Bank.Infrastructure.Data.Repositories.BankAccounts;
using Krt.Bank.Infrastructure.Data.Repositories.Users;
using Krt.Bank.Infrastructure.Messaging.EventPublisher;
using Microsoft.Extensions.Caching.Memory;
using test_krt_bank.EndPoints.BankAccounts;
using test_krt_bank.EndPoints.User;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<InMemoryBankAccountRepository>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

// Cache
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IBankAccountRepository>(provider =>
{
    var innerRepo = provider.GetRequiredService<InMemoryBankAccountRepository>();
    var cache = provider.GetRequiredService<IMemoryCache>();
    return new CachedBankAccountRepository(innerRepo, cache);
});



builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<GetUsersHandler>();
builder.Services.AddScoped<SaveBankAccountHandler>();
builder.Services.AddScoped<GetBankAccountHandler>();
builder.Services.AddScoped<GetBankAccountsHandler>();
builder.Services.AddScoped<DisableBankAccountsHandler>();
builder.Services.AddScoped<SaveBankAccountValidator>();
builder.Services.AddScoped<CreateUserValidator>();
builder.Services.AddScoped<BankAccountCreatedEventHandler>();

builder.Services.AddScoped<IEventPublisher, ConsoleEventPublisher>();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Bootstrappers
//ApplicationBootstrapper.RegisterApplicationServices(builder.Services);
//DbContextBootstrapper.RegisterDbContext(builder.Services, builder.Configuration);
//RepositoryBootstrapper.RegisterRepositoryServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = "swagger";
    });
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapBankAccountEndpoints();
app.MapUserEndpoints();
app.MapBankAccountEventsEndpoints();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();