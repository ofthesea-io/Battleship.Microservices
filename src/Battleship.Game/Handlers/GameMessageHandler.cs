namespace Battleship.Game.Handlers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Messages;
    using Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class GameMessageHandler : BackgroundService
    {
        private readonly IGameRepository gameRepository;
        private readonly IMessagePublisher messagePublisher;
        private IModel channel;
        private IConnection connection;

        public GameMessageHandler(IConfiguration configuration, IGameRepository gameRepository,
            IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
            this.gameRepository = gameRepository;

            Initialise();
        }

        private void Initialise()
        {
            var factory = new ConnectionFactory
            {
                HostName = this.messagePublisher.Host, UserName = this.messagePublisher.Username,
                Password = this.messagePublisher.Password
            };

            // create connection
            this.connection = factory.CreateConnection();

            // create channel
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(this.messagePublisher.Exchange, ExchangeType.Direct, true);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, this.messagePublisher.Queue);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // handle the received message
                try
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        var player = JsonConvert.DeserializeObject<Player>(content);
                        this.gameRepository.CreatePlayer(player.SessionToken, player.PlayerId);
                        this.channel.BasicAck(ea.DeliveryTag, true);
                    }
                }
                catch (Exception exp)
                {
                    var message = $"Battleship.Game:\r\n {exp.StackTrace}";
                    
                    this.channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            this.channel.BasicConsume(this.messagePublisher.Queue, false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            this.channel.Close();
            this.connection.Close();
            base.Dispose();
        }
    }
}