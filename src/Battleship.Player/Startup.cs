namespace Battleship.Player
{
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Repository;
    using Battleship.Microservices.Infrastructure.Messages;
    using Battleship.Player.Infrastructure;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        #region Fields

        private const string Origins = "http://localhost:4200";

        private readonly IConfiguration configuration;

        private readonly string database = "Database=Battleship.Player;";

        private string sqlConnectionString = string.Empty;

        #endregion

        #region Constructors

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #endregion

        #region Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.sqlConnectionString = this.configuration.GetConnectionString("BattleshipPlayerCN");
            string databaseConnection = $"{this.sqlConnectionString}{this.database}";

            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // add message publisher classes
            IConfigurationSection configSection = this.configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];
            string exchange = configSection["Exchange"];
            string queue = configSection["Queue"];

            services.AddTransient<IMessagePublisher>(sp => new MessagePublisher(host, userName, password, exchange, queue));
            services.AddSingleton<IPlayerRepository>(new PlayerRepository(databaseConnection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Initialisation.Setup(this.sqlConnectionString);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        #endregion
    }
}