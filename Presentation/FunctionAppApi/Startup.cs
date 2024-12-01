using Application;
using Application.Interfaces;
using Application.Interfaces.PaymentService;
using Application.Services;
using Application.Services.PaymentService;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionAppApi;
public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddSingleton<IUserContextService, UserContextService>();
        services.AddSingleton<IPaymentGatewayService, PaymentGatewayService>();
    }
}
