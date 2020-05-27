namespace Battleship.Warehouse
{
    using System;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Repository;
    using Battleship.Warehouse.Communication;
    using Battleship.Warehouse.Infrastructure;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        #region Methods

        public static async Task Main(string[] args)
        {
            Console.Write("WareHouse Microservice started.");

            IHost host = Program.CreateHostBuilder(args).Build();
            await host.RunAsync();

            Console.ReadLine();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder().ConfigureAppConfiguration((hostContext, config) => { config.AddJsonFile("appsettings.json", false); }).ConfigureServices(
                (hostContext, services) =>
                    {
                        IConfigurationSection sqlConnectionString = hostContext.Configuration.GetSection("ConnectionStrings");
                        Initialisation.Setup(sqlConnectionString["BattleshipWarehouseCN"]);
                        string database = "Database=Battleship.WareHouse;";
                        string databaseConnection = $"{sqlConnectionString}{database}";
                        IWareHouseRepository warehouseRepository = new WareHouseRepository(databaseConnection);
                        services.AddTransient(
                            svc =>
                                {
                                    IConfigurationSection configSection = hostContext.Configuration.GetSection("RabbitMQ");
                                    string host = configSection["Host"];
                                    string username = configSection["UserName"];
                                    string password = configSection["Password"];
                                    string exchange = configSection["Exchange"];
                                    string rpcQueue = configSection["RpcQueue"];

                                    return new RpcServerHandler(host, username, password, exchange, rpcQueue, warehouseRepository);
                                });
                    }).UseConsoleLifetime();

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
                WareHousing.IntervalInDays(
                    23,
                    59,
                    1,
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