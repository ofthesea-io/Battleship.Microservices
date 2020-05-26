namespace Battleship.Statistics.Handlers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;
    using Battleship.Statistics.Infrastructure;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    using Newtonsoft.Json;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class StatisticsHandler : BackgroundService
    {
        #region Fields

        private readonly IConfiguration configuration;

        private readonly IMessagePublisher messagePublisher;

        private readonly IStatisticsRepository statisticsRepository;

        private IModel channel;

        private IConnection connection;

        #endregion

        #region Constructors

        public StatisticsHandler(IConfiguration configuration, IStatisticsRepository statisticsRepository, IMessagePublisher messagePublisher)
        {
            this.configuration = configuration;
            this.statisticsRepository = statisticsRepository;
            this.messagePublisher = messagePublisher;
            this.Initialise();
        }

        #endregion

        #region Methods

        public override void Dispose()
        {
            this.channel.Close();
            this.connection.Close();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += async (sender, args) =>
                {
                    // received message
                    string content = Encoding.UTF8.GetString(args.Body.ToArray());

                    // handle the received message
                    try
                    {
                        if (!string.IsNullOrEmpty(content))
                        {
                            Statistics statistics = JsonConvert.DeserializeObject<Statistics>(content);
                            if (statistics != null)
                            {
                                /* Calculate user Hit ratio */
                                double winningPercentage;

                                // 1. Get player statistics by email address
                                Statistics statistic = await this.statisticsRepository.GetPlayerByEmail(statistics.Email);

                                // 2. Calculate the Hit ratio:  hit / (hit + miss) * 100
                                double winningRatio = statistics.ScoreCard.Hit / (statistics.ScoreCard.Hit + statistics.ScoreCard.Miss) * 100;

                                // 3. if result found in step one, add step 1 and 2 together and divide by 2
                                if (statistic != null)
                                    winningPercentage = Math.Round((winningRatio + statistics.WinningPercentage / 2), 2);
                                else
                                    winningPercentage = Math.Round(winningRatio, 2);
                                
                                // Save step 3 (or 2) to database by email address
                                if (statistic != null) statistic.WinningPercentage = winningPercentage;
                                await this.statisticsRepository.SaveStatistics(statistic);

                                this.channel.BasicAck(args.DeliveryTag, true);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string message = $"Battleship.Board: {e.Message}{Environment.NewLine}{e.StackTrace}";
                    }
                };

            this.channel.BasicConsume(this.messagePublisher.Queue, false, consumer);
            return Task.CompletedTask;
        }

        private void Initialise()
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = this.messagePublisher.Host, UserName = this.messagePublisher.Username, Password = this.messagePublisher.Password };

            // create connection
            this.connection = factory.CreateConnection();

            // create channel
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(this.messagePublisher.Exchange, ExchangeType.Direct, true);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, this.messagePublisher.Queue);
        }

        #endregion
    }
}