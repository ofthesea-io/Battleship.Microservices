namespace Battleship.Statistics.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Statistics.Communication;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ContextBase
    {
        #region Fields

        private readonly IRpcClient rpcClient;

        #endregion

        #region Constructors

        public StatisticController(IMessagePublisher messagePublisher, IRpcClient rpcClient)
            : base(messagePublisher)
        {
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
                CancellationToken token = new CancellationToken(false);
                string result = await this.rpcClient.CallAsync(token);
                return this.Ok();
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}