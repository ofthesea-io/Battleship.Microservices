namespace Battleship.Game.Handlers
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;

    using Battleship.Microservices.Core.Messages;

    using Microsoft.Extensions.Hosting;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class PlayerHandler
    {
        #region Fields

        private readonly IMessagePublisher messagePublisher;

        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();

        private IModel channel;

        private IConnection connection;

        private EventingBasicConsumer consumer;

        private IBasicProperties props;

        private string replyQueueName;

        #endregion

        #region Constructors

        public PlayerHandler(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;

            this.Initialise();
        }

        #endregion

        #region Methods

        public string GetPlayer(string sessionToken)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(sessionToken);
            this.channel.BasicPublish(this.messagePublisher.Exchange, "Player", this.props, messageBytes);

            this.channel.BasicConsume(consumer: this.consumer, queue: this.replyQueueName, autoAck: true);

            return this.respQueue.Take();
        }

        public void Close()
        {
            this.connection.Close();
        }

        private void Initialise()
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = this.messagePublisher.Host, UserName = this.messagePublisher.Username, Password = this.messagePublisher.Password };

            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();
            this.replyQueueName = this.channel.QueueDeclare().QueueName;
            this.consumer = new EventingBasicConsumer(this.channel);

            this.props = this.channel.CreateBasicProperties();
            string correlationId = Guid.NewGuid().ToString();
            this.props.CorrelationId = correlationId;
            this.props.ReplyTo = this.replyQueueName;

            this.consumer.Received += (model, ea) =>
                {
                    ReadOnlyMemory<byte> body = ea.Body;
                    string response = Encoding.UTF8.GetString(body.ToArray());
                    if (ea.BasicProperties.CorrelationId == correlationId) this.respQueue.Add(response);
                };
        }

        #endregion
    }
}