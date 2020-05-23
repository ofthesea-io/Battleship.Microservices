namespace Battleship.Statistics.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Statistics.Infrastructure;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ContextBase
    {
        #region Fields

        private readonly IStatisticsRepository statisticsRepository;

        #endregion

        #region Constructors

        public StatisticController(IMessagePublisher messagePublisher, IStatisticsRepository statisticsRepository)
            : base(messagePublisher)
        {
            this.statisticsRepository = statisticsRepository;
        }

        #endregion

        #region Methods

        [HttpGet]
        public string Get()
        {
            return "ScoreCard API started.";
        }

        [HttpGet]
        [Route("GetTopTenPlayers")]
        public async Task<ActionResult> GetTopTenPlayers()
        {
            try
            { 
                var result = await this.statisticsRepository.GetTopTenPlayers();
                return this.Ok(result);
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