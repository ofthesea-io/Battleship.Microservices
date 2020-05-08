namespace Battleship.Statistics.Communication
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RpcClient : IRpcClient
    {
        #region Fields

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper =
            new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        private readonly IModel channel;

        private readonly IConnection connection;
        private readonly EventingBasicConsumer consumer;
        private readonly string exchange;
        private readonly string host;
        private readonly string password;
        private readonly string replyQueueName;
        private readonly string rpcQueue;
        private readonly string username;

        #endregion

        #region Constructors

        public RpcClient(string host, string username, string password, string exchange, string rpcQueue,
            string datbaseString)
        {
            this.host = host;
            this.username = username;
            this.password = password;
            this.exchange = exchange;
            this.rpcQueue = rpcQueue;

            var factory = new ConnectionFactory
            {
                HostName = this.host, UserName = this.username, Password = this.password
            };

            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();
            this.replyQueueName = this.channel.QueueDeclare().QueueName;
            this.consumer = new EventingBasicConsumer(this.channel);
            this.consumer.Received += (model, ea) =>
            {
                if (!this.callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string> tcs)) return;
                byte[] body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };
        }

        #endregion

        #region Methods

        public Task<string> CallAsync(CancellationToken cancellationToken = default)
        {
            var props = this.channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = this.replyQueueName;

            var message = "hello world !!!!";

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            this.callbackMapper.TryAdd(correlationId, tcs);

            this.channel.BasicPublish(
                string.Empty,
                this.rpcQueue,
                props,
                messageBytes);

            this.channel.BasicConsume(
                consumer: this.consumer,
                queue: this.replyQueueName,
                autoAck: true);

            cancellationToken.Register(() => this.callbackMapper.TryRemove(correlationId, out TaskCompletionSource<string> tmp));
            return tcs.Task;
        }

        private void Close()
        {
            this.connection.Close();
        }

        #endregion
    }
}