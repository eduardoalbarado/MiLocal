using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FunctionAppApi;
class Program
{
    static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json", true);
                builder.AddJsonFile("local.settings.json", true);
            })
            .ConfigureServices((context, services) =>
            {
                Startup startup = new Startup(context.Configuration);
                startup.ConfigureServices(services);
            })
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services =>
            {
                services.AddHttpClient(); // Add any additional services needed for your middleware
                services.AddTransient<AuthenticationMiddleware>(); // Register the AuthenticationMiddleware
            })
            .ConfigureFunctionsWorkerDefaults((context, workerBuilder) =>
            {
                // Add the middleware to the pipeline
                workerBuilder.UseMiddleware<AuthenticationMiddleware>();
            })
            //.ConfigureOpenApi()
            .Build();

        await host.RunAsync();
    }
}