using Application.Interfaces;
using Application.Interfaces.PaymentService;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Repository;
using Infrastructure.Services;
using Infrastructure.Services.PaymentService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string environment = configuration.GetValue<string>("AZURE_FUNCTIONS_ENVIRONMENT") ?? "Development";
        bool isDevEnvironment = environment.Equals("Development", StringComparison.OrdinalIgnoreCase);
        string databaseProvider = configuration.GetValue<string>("DatabaseProvider") ?? "Sqlite";

        services.AddTransient<IDateTime, DateTimeService>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseInMemoryDatabase("AdminDb"));
        }
        else if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            // Use SQL Server with connection string from configuration
            string sqlServerConnectionString = configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' is not defined in the configuration.");
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseSqlServer(sqlServerConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");  // Optional: set migrations schema
                })
                .EnableSensitiveDataLogging(isDevEnvironment));
        }
        else
        {
            // Default to SQLite with file path based on environment
            string sqliteConnectionString = isDevEnvironment
                ? "Data Source=Data\\MiLocalDb.db"
                : "Data Source=C:/home/MiLocalDb.db";

            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseSqlite(sqliteConnectionString)
                .EnableSensitiveDataLogging(isDevEnvironment)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll));
        }
        // Registering IDbContext as MiLocalDbContext
        services.AddScoped<IDbContext, MiLocalDbContext>();

        // Registering the repositories
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

        // Registering the UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IPaymentGatewayClient, MercadoPagoPaymentGatewayClient>();

        return services;
    }
}
