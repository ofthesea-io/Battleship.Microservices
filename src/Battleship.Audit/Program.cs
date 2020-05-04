namespace Battleship.Audit
{
    using System;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Messages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("Audit Microservice started.");

            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();

            Console.ReadLine();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.json", false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient(svc =>
                    {
                        var configSection = hostContext.Configuration.GetSection("RabbitMQ");
                        var host = configSection["Host"];
                        var username = configSection["UserName"];
                        var password = configSection["Password"];
                        var exchange = configSection["Exchange"];
                        var queue = configSection["Queue"];

                        return new MessagePublisher(host, username, password, exchange, queue);
                    });
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }
}