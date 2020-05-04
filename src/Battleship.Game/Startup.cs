namespace Battleship.Game
{
    using Battleship.Microservices.Infrastructure.Messages;
    using Battleship.Microservices.Infrastructure.Repository;
    using Handlers;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly string database = "Database=Battleship.Game;";
        private string sqlConnectionString = string.Empty;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.sqlConnectionString = this.configuration.GetConnectionString("BattleshipGameCN");
            var databaseConnection = $"{this.sqlConnectionString}{this.database}";

            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var configSection = this.configuration.GetSection("RabbitMQ");
            var host = configSection["Host"];
            var username = configSection["UserName"];
            var password = configSection["Password"];
            var exchange = configSection["Exchange"];
            var queue = configSection["Queue"];

            var auditSection = this.configuration.GetSection("RabbitMQ");
            var auditQueue = configSection["AuditQueue"];
            var auditPath = configSection["auditPath"];

            services.AddSingleton<IGameRepository>(new GameRepository(databaseConnection));
            services.AddTransient<IMessagePublisher>(sp =>
                new MessagePublisher(host, username, password, exchange, queue));
            services.AddHostedService<GameMessageHandler>();
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
                options => options.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
            );

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}