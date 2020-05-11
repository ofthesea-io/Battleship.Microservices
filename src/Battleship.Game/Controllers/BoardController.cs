namespace Battleship.Game.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Battleship.Game.Board;
    using Battleship.Game.Infrastructure;
    using Battleship.Game.Models;
    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Primitives;

    using Newtonsoft.Json;

    /// <summary>
    ///     The game board generation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ContextBase, IBoardController
    {
        #region Fields

        private readonly IGameRepository gameRepository;

        private readonly IGridGenerator gridGenerator;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, DefaultValueHandling = DefaultValueHandling.Ignore };

        private readonly IStringLocalizer<BoardController> localizer;

        private readonly IMessagePublisher messagePublisher;

        #endregion

        #region Constructors

        public BoardController(IGameRepository gameRepository, IMessagePublisher messagePublisher, IStringLocalizer<BoardController> localizer)
            : base(messagePublisher)
        {
            this.gridGenerator = GridGenerator.Instance();
            this.gameRepository = gameRepository;
            this.messagePublisher = messagePublisher;
            this.localizer = localizer;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GenerateBoard")]
        public async Task<ActionResult<GamingGrid>> GetGamingGrid()
        {
            try
            {
                GamingGrid result = await Task.Run(() =>
                {
                    GamingGrid gamingGrid = new GamingGrid { X = this.GetXAxis(), Y = this.GetYAxis() };

                    return gamingGrid;
                });

                return result;
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        [Route("UserInput")]
        public async Task<ActionResult> UserInput([FromBody] PlayerCommand playerCommand)
        {
            try
            {
                string sessionToken = this.IsAuthenticated(this.HttpContext);
                if (string.IsNullOrEmpty(sessionToken))
                    return this.StatusCode(StatusCodes.Status401Unauthorized);

                if (playerCommand.Coordinate.X == 0 || playerCommand.Coordinate.Y == 0 || string.IsNullOrEmpty(sessionToken))
                    throw new ArgumentException();

                string coordinates = await this.gameRepository.GetShipCoordinates(sessionToken);

                Dictionary<Coordinate, Segment> shipCoordinates = JsonConvert.DeserializeObject<KeyValuePair<Coordinate, Segment>[]>(coordinates, this.jsonSerializerSettings).ToDictionary(kv => kv.Key, kv => kv.Value);

                KeyValuePair<Coordinate, Segment> shipCoordinate = shipCoordinates.FirstOrDefault(q => q.Key.X == playerCommand.Coordinate.X && q.Key.Y == playerCommand.Coordinate.Y);

                playerCommand.ScoreCard.IsHit = false;
                if (shipCoordinate.Value != null)
                {
                    shipCoordinate.Value.Ship.ShipHit++;
                    string updateShipCoordinates = JsonConvert.SerializeObject(shipCoordinates.ToArray(), Formatting.Indented, this.jsonSerializerSettings);
                    await this.gameRepository.UpdateShipCoordinates(updateShipCoordinates, sessionToken);
                    playerCommand.ScoreCard.Hit++;
                    playerCommand.ScoreCard.Message = this.localizer["Boom! You hit a ship!"];
                    playerCommand.ScoreCard.IsHit = true;
                    if (shipCoordinate.Value.Ship.IsShipSunk)
                    {
                        playerCommand.ScoreCard.Sunk++;
                        playerCommand.ScoreCard.Message = this.localizer["Ship sunk!"];
                    }
                }
                else
                {
                    playerCommand.ScoreCard.Miss++;
                    playerCommand.ScoreCard.Message = this.localizer["Sorry you missed, try again!"];
                }

                playerCommand.ScoreCard.Total++;
                playerCommand.ScoreCard.SessionToken = sessionToken;

                await this.messagePublisher.PublishMessageAsync(JsonConvert.SerializeObject(playerCommand.ScoreCard), "ScoreCard");
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return this.Ok(JsonConvert.SerializeObject(playerCommand.ScoreCard));
        }

        [HttpGet]
        public string Get()
        {
            return "Board API started.";
        }

        [HttpGet]
        [Route("StartGame")]
        public async Task<ActionResult> StartGame(int numberOfShips)
        {
            try
            {
                if (numberOfShips == 0) return this.BadRequest();

                string sessionToken = this.IsAuthenticated(this.HttpContext);
                if (string.IsNullOrEmpty(sessionToken)) return this.StatusCode(StatusCodes.Status401Unauthorized);

                await this.gameRepository.StartGame(sessionToken, numberOfShips);

                this.Log($"Game started on {sessionToken}");

                // Publish the message to the ScoreCard Queue
                ScoreCard scoreCard = new ScoreCard
                                          {
                                              SessionToken = sessionToken,
                                              Message = this.localizer["Let the games begin!"].Value,
                                              Hit = 0,
                                              Miss = 0,
                                              Sunk = 0,
                                              IsCompleted = false,
                                              IsHit = false,
                                              Total = 0
                                          };
                string serializedScoreCard = JsonConvert.SerializeObject(scoreCard);
                await this.messagePublisher.PublishMessageAsync(serializedScoreCard, "ScoreCard");

                return this.StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private IEnumerable<string> GetXAxis()
        {
            try
            {
                return this.gridGenerator.GetAlphaColumnChars();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IEnumerable<int> GetYAxis()
        {
            try
            {
                return this.gridGenerator.GetNumericRows();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string IsAuthenticated(HttpContext httpContext)
        {
            string result = string.Empty;
            if (httpContext.Request.Headers.TryGetValue("Authorization", out StringValues values)) result = values.FirstOrDefault();

            return result;
        }

        #endregion
    }
}