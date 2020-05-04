namespace Battleship.Score
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
        public IConfiguration configuration;
        private string sqlConnectionString = string.Empty;

        private readonly string database = "Database=Battleship.ScoreCard;";

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            this.sqlConnectionString = this.configuration.GetConnectionString("BattleshipGameCN");
            var databaseConnection = $"{this.sqlConnectionString}{this.database}";


            // add message publisher classes
            var configSection = this.configuration.GetSection("RabbitMQ");
            var host = configSection["Host"];
            var username = configSection["UserName"];
            var password = configSection["Password"];
            var exchange = configSection["Exchange"];
            var queue = configSection["Queue"];

            services.AddSingleton<IScoreCardRepository>(new ScoreCardRepository(databaseConnection));
            services.AddTransient<IMessagePublisher>(sp => new MessagePublisher(host, username, password, exchange, queue));
            services.AddHostedService<ScoreCardHandler>();

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Initialisation.Setup(this.sqlConnectionString);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();
            app.UseCors(options => options
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