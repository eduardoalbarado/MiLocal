using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTime, DateTimeService>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseInMemoryDatabase("AdminDb"));
        }
        else
        {
            services.AddDbContext<MiLocalDbContext>(options =>
                options.UseSqlite("Data Source=Data\\MiLocalDb.db"));
        }
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<IDbContext>());
        // Registering the repositories
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        // Registering the UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
