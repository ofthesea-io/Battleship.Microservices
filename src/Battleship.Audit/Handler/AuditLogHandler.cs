namespace Battleship.Audit.Handler
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Messages;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using Serilog;

    public class AuditLogHandler : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IMessagePublisher messagePublisher;

        private IModel channel;
        private IConnection connection;

        public AuditLogHandler(IConfiguration configuration, IMessagePublisher messagePublisher)
        {
            this.configuration = configuration;
            this.messagePublisher = messagePublisher;

            this.Initialise();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                try
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                    var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:ffffff} - {content}{Environment.NewLine}";
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.File(this.AuditPath)
                        .CreateLogger();

                    Log.Information(logMessage);
                }
                catch (Exception e)
                {
                    this.channel.BasicAck(ea.DeliveryTag, false);
                    Debug.WriteLine(e.StackTrace);
                }
            };

            this.channel.BasicConsume(this.messagePublisher.Queue, false, consumer);
            return Task.CompletedTask;
        }

        private void Initialise()
        {

            var factory = new ConnectionFactory
            {
                HostName = this.messagePublisher.Host,
                UserName = this.messagePublisher.Username,
                Password = this.messagePublisher.Password
            };

            this.AuditPath = Assembly.GetExecutingAssembly().ToString() + Path.DirectorySeparatorChar + "AuditLog";

            if (!Directory.Exists(AuditPath))
                Directory.CreateDirectory(AuditPath);

            // create connection
            this.connection = factory.CreateConnection();

            // create channel
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(this.messagePublisher.Exchange, ExchangeType.Direct, true);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, "");
        }

        public string AuditPath { get; private set; }
    }

    
}