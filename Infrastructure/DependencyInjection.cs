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

        ConfigureDatabase(services, configuration, isDevEnvironment, databaseProvider);



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

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration, bool isDevEnvironment, string databaseProvider)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseInMemoryDatabase("AdminDb"));
        }
        else if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            string sqlServerConnectionString = configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' is not defined in the configuration.");
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseSqlServer(sqlServerConnectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");  // Optional: set migrations schema
                })
                .EnableSensitiveDataLogging(isDevEnvironment));
        }
        else if (databaseProvider.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
        {
            string sqliteConnectionString = configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string 'SQLite' is not defined in the configuration.");
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseSqlite(sqliteConnectionString)
                .EnableSensitiveDataLogging(isDevEnvironment)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll));
        }
        else
        { throw new ArgumentException("Not a valid database type"); }
    }
}
