namespace Battleship.Audit.Handlers
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;

    using Infrastructure;
    using Microservices.Infrastructure.Messages;

    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class AuditMessageHandler : BackgroundService
    {

        private readonly IAuditRepository auditRepository;
        private readonly IMessagePublisher messagePublisher;

        private IModel channel;
        private IConnection connection;

        public AuditMessageHandler(IAuditRepository auditRepository, IMessagePublisher messagePublisher)
        {
            this.auditRepository = auditRepository;
            this.messagePublisher = messagePublisher;

            this.Initialise();
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
            consumer.Received += (ch, ea) =>
            {
                // received message
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // handle the received message
                try
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        var auditMessage = JsonConvert.DeserializeObject<Audit>(content);
                        this.auditRepository.SaveAuditMessage(auditMessage.AuditType, auditMessage.Message, auditMessage.Username);
                        this.channel.BasicAck(ea.DeliveryTag, true);
                    }
                }
                catch (Exception)
                {
                    this.channel.BasicAck(ea.DeliveryTag, false);
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

            // create connection
            this.connection = factory.CreateConnection();

            // create channel
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(this.messagePublisher.Exchange, ExchangeType.Direct, true);
            this.channel.QueueBind(this.messagePublisher.Queue, this.messagePublisher.Exchange, this.messagePublisher.Queue);
        }
    }
}