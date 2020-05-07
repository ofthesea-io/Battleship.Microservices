namespace Battleship.Score.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microservices.Infrastructure.Messages;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json;

    /// <summary>
    ///     The game board generation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreCardController : ControllerBase, IScoreCardController
    {
        #region Fields

        private readonly IMessagePublisher    messagePublisher;
        private readonly IScoreCardRepository scoreCardRepository;

        #endregion

        #region Constructors

        public ScoreCardController(IScoreCardRepository scoreCardRepository, IMessagePublisher messagePublisher)
        {
            this.scoreCardRepository = scoreCardRepository;
            this.messagePublisher = messagePublisher;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetPlayerScoreCard")]
        public async Task<ActionResult> GetPlayerScoreCard()
        {
            try
            {
                var card = string.Empty;
                string sessionToken = this.HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(sessionToken)) return this.BadRequest();

                var scoreCard = await this.scoreCardRepository.GetPlayerScoreCard(sessionToken);

                // if scorecard has not been created, just return 200
                if (string.IsNullOrEmpty(scoreCard)) return this.StatusCode(StatusCodes.Status200OK);
                card = JsonConvert.SerializeObject(scoreCard);

                if (string.IsNullOrEmpty(card)) return this.StatusCode(StatusCodes.Status400BadRequest);

                return this.Ok(card);
            }
            catch (SqlException e)
            {
                var message = $"Battleship.SoreCard SQL exception: {e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                var message = $"Battleship.SoreCard: {e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}