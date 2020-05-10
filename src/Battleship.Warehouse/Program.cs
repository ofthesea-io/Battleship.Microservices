namespace Battleship.Warehouse
{
    using System;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Repository;

    using Communication;
    using Infrastructure;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        #region Methods

        public static async Task Main(string[] args)
        {
            Console.Write("WareHouse Microservice started.");

            var host = Program.CreateHostBuilder(args).Build();
            await host.RunAsync();

            Console.ReadLine();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
               .ConfigureAppConfiguration((hostContext, config) => { config.AddJsonFile("appsettings.json", false); })
               .ConfigureServices((hostContext, services) =>
                {
                    var sqlConnectionString = hostContext.Configuration.GetSection("ConnectionStrings");
                    Initialisation.Setup(sqlConnectionString["BattleshipWarehouseCN"]);
                    var database = "Database=Battleship.WareHouse;";
                    var databaseConnection = $"{sqlConnectionString}{database}";
                    IWareHouseRepository warehouseRepository = new WareHouseRepository(databaseConnection);
                    services.AddTransient(svc =>
                    {
                        var configSection = hostContext.Configuration.GetSection("RabbitMQ");
                        var host = configSection["Host"];
                        var username = configSection["UserName"];
                        var password = configSection["Password"];
                        var exchange = configSection["Exchange"];
                        var rpcQueue = configSection["RpcQueue"];

                        return new RpcServer(host, username, password, exchange, rpcQueue, warehouseRepository);
                    });
                })
               .UseConsoleLifetime();

            return hostBuilder;
        }

        /// <summary>
        ///     This commonly warehouse that data for analysis. For the POC
        ///     we will clean out the database and only keep the hot data for
        ///     the day.
        /// </summary>
        private void WarehouseData()
        {
            try
            {
                WareHousing.IntervalInDays(23, 59, 1,
                    () =>
                    {
                        // TODO - execute SQL Proc at midnight
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}