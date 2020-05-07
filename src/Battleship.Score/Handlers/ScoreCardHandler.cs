namespace Battleship.Score.Handlers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microservices.Infrastructure.Messages;
    using Microservices.Infrastructure.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class ScoreCardHandler : BackgroundService
    {
        #region Fields

        private readonly IConfiguration       configuration;
        private readonly IMessagePublisher    messagePublisher;
        private readonly IScoreCardRepository scoreCardRepository;
        private          IModel               channel;
        private          IConnection          connection;

        #endregion

        #region Constructors

        public ScoreCardHandler(IConfiguration configuration, IScoreCardRepository scoreCardRepository,
            IMessagePublisher messagePublisher)
        {
            this.configuration = configuration;
            this.scoreCardRepository = scoreCardRepository;
            this.messagePublisher = messagePublisher;
            this.Initialise();
        }

        #endregion

        #region Methods

        private void Initialise()
        {
            var factory = new ConnectionFactory
            {
                HostName = this.messagePublisher.Host,
                UserName = this.messagePublisher.Username,
                Password = this.messagePublisher.Password
            };

            // create connection
            this.connection = factory.CreateConnection();

            // create channel
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(this.messagePublisher.Exchange, ExchangeType.Direct, true);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, this.messagePublisher.Queue);
        }

        public override void Dispose()
        {
            this.channel.Close();
            this.connection.Close();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += (sender, args) =>
            {
                // received message
                var content = Encoding.UTF8.GetString(args.Body.ToArray());

                // handle the received message
                try
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        var scoreCard = JsonConvert.DeserializeObject<ScoreCard>(content);
                        if (scoreCard != null && !string.IsNullOrEmpty(scoreCard.Message))
                        {
                            var data = JsonConvert.SerializeObject(scoreCard);
                            this.scoreCardRepository.ManagePlayerScoreCard(scoreCard.SessionToken, data);
                            this.channel.BasicAck(args.DeliveryTag, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    var message = $"Battleship.Board: {e.Message}{Environment.NewLine}{e.StackTrace}";
                }
            };

            this.channel.BasicConsume(this.messagePublisher.Queue, false, consumer);
            return Task.CompletedTask;
        }

        #endregion
    }
}