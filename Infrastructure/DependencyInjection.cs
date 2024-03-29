﻿using Application.Interfaces;
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

        services.AddTransient<IDateTime, DateTimeService>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseInMemoryDatabase("AdminDb"));
        }
        else
        {
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseSqlite($"Data Source={(isDevEnvironment ? "Data\\MiLocalDb.db" : "C:/home/MiLocalDb.db")}"));
        }
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<IDbContext>());
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
