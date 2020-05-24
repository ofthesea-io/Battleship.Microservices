namespace Battleship.Player.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Components;
    using Battleship.Microservices.Core.Messages;
    using Battleship.Microservices.Core.Utilities;
    using Battleship.Player.Infrastructure;
    using Battleship.Player.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    /// <summary>
    ///     Player Management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ContextBase, IPlayerController
    {
        #region Fields

        private readonly IMessagePublisher messagePublisher;

        private readonly IPlayerRepository playerRepository;

        #endregion

        #region Constructors

        public PlayerController(IPlayerRepository playerRepository, IMessagePublisher messagePublisher)
            : base(messagePublisher)
        {
            this.playerRepository = playerRepository;
            this.messagePublisher = messagePublisher;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("PlayerLogin")]
        public async Task<ActionResult> PlayerLogin([FromBody] Player player)
        {
            try
            {
                Player result = await this.playerRepository.PlayerLogin(player);
                if (result == null)
                {
                    this.Log($"401 Unauthorized {player.Email}", this.AuditLogQueue, AuditType.Warning);
                    return this.StatusCode(StatusCodes.Status401Unauthorized);
                }

                // publish the message to the GamePlay Queue
                result.SessionToken = this.GenerateToken(result.SessionGuid);
                await this.messagePublisher.PublishMessageAsync(JsonConvert.SerializeObject(result), "GamePlay");

                // convert the guid to a string
                result.SessionToken = this.GenerateToken(result.SessionGuid);

                // convert the session token
                string token = JsonConvert.SerializeObject(result);
                return this.Ok(token);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("IsAuthenticated")]
        public async Task<ActionResult> IsAuthenticated(string sessionId)
        {

            try
            {
                Authenticated result = await this.playerRepository.IsAuthenticated(sessionId);
                if (result == null) return this.StatusCode(StatusCodes.Status401Unauthorized);
                string data = JsonConvert.SerializeObject(result);
                return this.Ok(data);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CreatePlayer")]
        public async Task<ActionResult> CreatePlayer([FromBody] Player player)
        {
            try
            {
                Guid result = await this.playerRepository.CreatePlayer(player);
                if (result == Guid.Empty) return this.StatusCode(StatusCodes.Status400BadRequest);

                string token = this.GenerateToken(player.PlayerId);
                player.PlayerId = result;
                player.SessionToken = token;

                string data = JsonConvert.SerializeObject(player);

                return this.Ok(data);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetDemoPlayers")]
        public async Task<ActionResult> GetDemoPlayers()
        {
            try
            {
                IEnumerable<Player> result = await this.playerRepository.GetDemoPlayers();
                if (result == null) return this.BadRequest(false);

                string json = JsonConvert.SerializeObject(result);
                return this.Ok(json);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("DemoLogin")]
        public async Task<ActionResult> DemoLogin(Guid playerId)
        {
            try
            {
                if (playerId == Guid.Empty) return this.StatusCode(StatusCodes.Status403Forbidden);

                // session is created in the database
                Player player = await this.playerRepository.DemoLogin(playerId);
                if (player == null) return this.BadRequest(false);

                this.Log($"Demo Player {player.Firstname} {player.Lastname} Login");

                // publish the message to the GamePlay Queue
                player.SessionToken = this.GenerateToken(player.SessionGuid);
                string serializedPlayer = JsonConvert.SerializeObject(player);
                await this.messagePublisher.PublishMessageAsync(serializedPlayer, "GamePlay");

                return this.Ok(serializedPlayer);
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("PlayerLogout")]
        public async Task<ActionResult> PlayerLogout([FromBody] Player player)
        {
            try
            {
                this.Log($"Player {player.Firstname} {player.Lastname} Logout");
                bool result = await this.playerRepository.PlayerLogout(player.PlayerId);
                if (!result)
                {
                    string message = $"Logout failed {player.Firstname} {player.Lastname}";
                    await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                    return this.StatusCode(StatusCodes.Status500InternalServerError);
                }

                return this.Ok();
            }
            catch (Exception e)
            {
                this.Log(e);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public string Get()
        {
            return "Player API started.";
        }

        private string GenerateToken(Guid result)
        {
            byte[] sessionBytes = Encoding.UTF8.GetBytes(result.ToString());
            return Convert.ToBase64String(sessionBytes);
        }

        #endregion
    }
}