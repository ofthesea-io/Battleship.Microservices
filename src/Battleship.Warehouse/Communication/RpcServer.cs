namespace Battleship.Warehouse.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Extensions.Hosting;
    using Model;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RpcServer : BackgroundService, IRpcServer
    {
        #region Fields

        private readonly string exchange;
        private readonly string host;
        private readonly IWareHouseRepository iWareHouseRepository;
        private readonly string password;
        private readonly string rpcQueue;
        private readonly string username;

        #endregion

        #region Constructors

        public RpcServer(string host, string username, string password, string exchange, string rpcQueue,
            IWareHouseRepository iWareHouseRepository)
        {
            this.host = host;
            this.username = username;
            this.password = password;
            this.exchange = exchange;
            this.rpcQueue = rpcQueue;

            this.iWareHouseRepository = iWareHouseRepository;
        }

        #endregion

        #region Methods

        public void Execute()
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = this.host, UserName = this.username, Password = this.password
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(this.rpcQueue, false, false, false, null);
                    channel.BasicQos(0, 1, false);
                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(this.rpcQueue, false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;
                        try
                        {
                            IEnumerable<Player> players = this.iWareHouseRepository.GetTopTenPlayers();
                            var result = JsonConvert.SerializeObject(players);

                            byte[] responseBytes = Encoding.UTF8.GetBytes(result);
                            channel.BasicPublish(string.Empty, props.ReplyTo, replyProps, responseBytes);
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    };
                }
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}