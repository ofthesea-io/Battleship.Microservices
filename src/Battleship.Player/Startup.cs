namespace Battleship.Player
{
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Repository;

    using Infrastructure;
    using Microservices.Infrastructure.Messages;

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
            var databaseConnection = $"{this.sqlConnectionString}{this.database}";

            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // add message publisher classes
            var configSection = this.configuration.GetSection("RabbitMQ");
            var host = configSection["Host"];
            var userName = configSection["UserName"];
            var password = configSection["Password"];
            var exchange = configSection["Exchange"];
            var queue = configSection["Queue"];

            services.AddTransient<IMessagePublisher>(sp =>
                new MessagePublisher(host, userName, password, exchange, queue));
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

            app.UseCors(
                options => options.AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowAnyOrigin()
            );
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        #endregion
    }
}