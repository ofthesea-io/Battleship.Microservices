namespace Battleship.Score
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            Program.CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostContext, config) => { config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", false); }).UseStartup<Startup>();
        }

        #endregion
    }
}