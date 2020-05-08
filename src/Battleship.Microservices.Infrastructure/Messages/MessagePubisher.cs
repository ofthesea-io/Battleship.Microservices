namespace Battleship.Microservices.Infrastructure.Messages
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Components;
    using Polly;
    using RabbitMQ.Client;
    using Utilities;

    public class MessagePublisher : ComponentBase, IMessagePublisher
    {
        #region Constructors

        public MessagePublisher(
            string host,
            string username,
            string password,
            string exchange,
            string queue)
        {
            this.Host = host;
            this.Username = username;
            this.Password = password;
            this.Exchange = exchange;
            this.Queue = queue;
        }

        #endregion

        #region Properties

        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Exchange { get; set; }

        public string Queue { get; set; }

        #endregion

        #region Methods

        public Task PublishMessageAsync(string message, string queue)
        {
            return Task.Run(() => Policy.Handle<Exception>().WaitAndRetry(9, r => TimeSpan.FromSeconds(5.0), (e, s) => { }).Execute(() =>
            {
                using (var connection = new ConnectionFactory
                {
                    HostName = this.Host,
                    UserName = this.Username,
                    Password = this.Password
                }.CreateConnection())
                {
                    using (var model = connection.CreateModel())
                    {
                        model.ExchangeDeclare(this.Exchange, "direct", true);
                        model.QueueDeclare(queue, true, false, false, null);

                        model.QueueBind(queue, this.Exchange, queue);
                        byte[] bytes = Encoding.UTF8.GetBytes(message);

                        model.BasicPublish(this.Exchange, queue, null, bytes);
                    }
                }
            }));
        }

        public Task PublishAuditLogMessageAsync(AuditType auditType, string message)
        {
            return this.PublishMessageAsync(message, "AuditLog");
        }

        #endregion
    }
}