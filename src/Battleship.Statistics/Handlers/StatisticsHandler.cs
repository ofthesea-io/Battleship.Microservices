namespace Battleship.Statistics.Handlers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Battleship.Core.Messages;
    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Infrastructure.Core.Models;
    using Battleship.Statistics.Infrastructure;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    using Newtonsoft.Json;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    using Serilog;

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

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += async (sender, eventArgs) =>
                {
                    // received message
                    string content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

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
                                double hit = (double)statistics.ScoreCard.Hit;
                                double miss = (double)statistics.ScoreCard.Miss;
                                double winningRatio = hit / (hit + miss) * 100;

                                // 3. if result found in step one, add step 1 and 2 together and divide by 2
                                if (statistic != null)
                                {
                                    winningPercentage = Math.Round(winningRatio + statistics.WinningPercentage / 2, 2);
                                    statistics.CompletedGames++;
                                }
                                else
                                {
                                    winningPercentage = Math.Round(winningRatio, 2);
                                    statistics.CompletedGames = 1;
                                }

                                statistics.WinningPercentage = winningPercentage;

                                // Save step 3 (or 2) to database by email address
                                await this.statisticsRepository.SaveStatistics(statistics);

                                this.channel.BasicAck(eventArgs.DeliveryTag, true);
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        Log.Error(exp, exp.Message);
                        this.channel.BasicAck(eventArgs.DeliveryTag, false);
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
            this.channel.QueueDeclare(queue: this.messagePublisher.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, this.messagePublisher.Queue);
        }

        #endregion
    }
}