namespace Battleship.Game.Handlers
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;

    using Battleship.Microservices.Core.Messages;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class PlayerHandler
    {
        #region Fields

        private readonly IModel channel;

        private readonly IConnection connection;

        private readonly EventingBasicConsumer consumer;

        private readonly string correlationId;

        private readonly IMessagePublisher messagePublisher;

        private readonly IBasicProperties props;

        private readonly string replyQueueName;

        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();

        #endregion

        #region Constructors

        public PlayerHandler(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;

            ConnectionFactory factory = new ConnectionFactory { HostName = this.messagePublisher.Host, UserName = this.messagePublisher.Username, Password = this.messagePublisher.Password };

            // Create the connection
            this.connection = factory.CreateConnection();

            // create the channel
            this.channel = this.connection.CreateModel();

            this.replyQueueName = this.channel.QueueDeclare().QueueName;
            this.consumer = new EventingBasicConsumer(this.channel);

            this.correlationId = Guid.NewGuid().ToString();

            this.props = this.channel.CreateBasicProperties();
            this.props.CorrelationId = this.correlationId;
            this.props.ReplyTo = this.replyQueueName;

            this.consumer.Received += (model, ea) =>
                {
                    ReadOnlyMemory<byte> body = ea.Body;
                    string response = Encoding.UTF8.GetString(body.ToArray());
                    if (ea.BasicProperties.CorrelationId == this.correlationId)
                        this.respQueue.Add(response);
                };
        }

        #endregion

        #region Methods

        public string GetPlayer(Guid playerId)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(playerId.ToString());
            this.channel.BasicPublish(string.Empty, "Player", this.props, messageBytes);
            this.channel.BasicConsume(consumer: this.consumer, queue: this.replyQueueName, autoAck: true);

            return this.respQueue.Take();
        }

        public void Close()
        {
            this.channel.Close();
            this.connection.Close();
        }

        #endregion
    }
}