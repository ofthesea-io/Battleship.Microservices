namespace Battleship.Player.Handlers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Messages;
    using Battleship.Player.Infrastructure;
    using Battleship.Player.Models;

    using Microsoft.Extensions.Hosting;

    using Newtonsoft.Json;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    using Serilog;

    public class PlayerRpcHandler : BackgroundService
    {
        #region Fields

        private readonly IConnection connection;

        private readonly IMessagePublisher messagePublisher;

        private readonly IPlayerRepository playerRepository;

        private IModel channel;

        #endregion

        #region Constructors

        public PlayerRpcHandler(IMessagePublisher messagePublisher, IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
            this.messagePublisher = messagePublisher;

            ConnectionFactory factory = new ConnectionFactory { HostName = this.messagePublisher.Host, UserName = this.messagePublisher.Username, Password = this.messagePublisher.Password };

            // create connection
            this.connection = factory.CreateConnection();

            // create channel
            this.channel = this.connection.CreateModel();
            this.channel.ExchangeDeclare(this.messagePublisher.Exchange, ExchangeType.Direct, true);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, this.messagePublisher.Queue);
        }

        #endregion

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            this.channel = this.connection.CreateModel();

            EventingBasicConsumer consumer = new EventingBasicConsumer(this.channel);
            this.channel.BasicConsume(this.messagePublisher.Queue, false, consumer);

            consumer.Received += async (model, ea) =>
            {
                string json = string.Empty;
                byte[] body = ea.Body.ToArray();
                IBasicProperties props = ea.BasicProperties;
                IBasicProperties replyProps = this.channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                try
                {
                    string message = Encoding.UTF8.GetString(body);
                    if (!string.IsNullOrEmpty(message))
                    {
                        Guid.TryParse(message, out Guid playerId);
                        Player player = await this.playerRepository.GetPlayer(playerId);
                        json = JsonConvert.SerializeObject(player);
                    }
                }
                catch (Exception exp)
                {
                    Log.Error(exp, exp.Message);
                    this.channel.BasicAck(ea.DeliveryTag, false);
                }
                finally
                {
                    byte[] responseBytes = Encoding.UTF8.GetBytes(json);
                    this.channel.BasicPublish(string.Empty, props.ReplyTo, replyProps, responseBytes);
                    this.channel.BasicAck(ea.DeliveryTag, false);
                }
            };
        }

        #endregion
    }
}