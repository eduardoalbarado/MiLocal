using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
            //.ConfigureOpenApi()
            .Build();

        await host.RunAsync();
    }
}
