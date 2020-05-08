namespace Battleship.Statistics.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Communication;
    using Microservices.Infrastructure.Messages;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        #region Fields

        private readonly IMessagePublisher messagePublisher;
        private readonly IRpcClient rpcClient;

        #endregion

        #region Constructors

        public StatisticController(IMessagePublisher messagePublisher, IRpcClient rpcClient)
        {
            this.messagePublisher = messagePublisher;
            this.rpcClient = rpcClient;
        }

        #endregion

        #region Methods

        [HttpGet]
        public string Get()
        {
            return "ScoreCard API started.";
        }

        [HttpGet]
        [Route("GetTopPlayers")]
        public async Task<ActionResult> GetTopPlayers()
        {
            try
            {
                var token = new CancellationToken(false);
                var result = await this.rpcClient.CallAsync(token);
                return this.Ok();
            }
            catch (Exception e)
            {
                var message = $"Battleship.Statistic: {e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}