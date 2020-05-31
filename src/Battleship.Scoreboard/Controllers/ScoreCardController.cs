namespace Battleship.Scoreboard.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Battleship.Core.Messages;
    using Battleship.Infrastructure.Core.Components;
    using Battleship.Infrastructure.Core.Messages;
    using Battleship.Score.Infrastructure;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;

    using Newtonsoft.Json;

    /// <summary>
    ///     The game board generation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreCardController : ContextBase, IScoreCardController
    {
        #region Fields

        private readonly IScoreCardRepository scoreCardRepository;

        #endregion

        #region Constructors

        public ScoreCardController(IScoreCardRepository scoreCardRepository, IMessagePublisher messagePublisher)
            : base(messagePublisher)
        {
            this.scoreCardRepository = scoreCardRepository;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetPlayerScoreCard")]
        public async Task<ActionResult> GetPlayerScoreCard()
        {
            try
            {
                string card = string.Empty;
                string sessionToken = this.HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(sessionToken)) return this.BadRequest();

                string scoreCard = await this.scoreCardRepository.GetPlayerScoreCard(sessionToken);

                // if scorecard has not been created, just return 200
                if (string.IsNullOrEmpty(scoreCard)) return this.StatusCode(StatusCodes.Status200OK);
                card = JsonConvert.SerializeObject(scoreCard);

                if (string.IsNullOrEmpty(card)) return this.StatusCode(StatusCodes.Status400BadRequest);

                return this.Ok(card);
            }
            catch (SqlException e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
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