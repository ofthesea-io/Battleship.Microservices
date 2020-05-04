namespace Battleship.Player.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Messages;
    using Infrastructure;
    using Microservices.Infrastructure.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;

    /// <summary>
    ///     Player Management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase, IPlayerController
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly IPlayerRepository playerRepository;

        public PlayerController(IPlayerRepository playerRepository, IMessagePublisher messagePublisher)
        {
            this.playerRepository = playerRepository;
            this.messagePublisher = messagePublisher;
        }

        [HttpPost]
        [Route("PlayerLogin")]
        public async Task<ActionResult> PlayerLogin([FromBody] Player player)
        {
            try
            {
                var result = await this.playerRepository.PlayerLogin(player);
                if (result == null)
                {
                    var message = $"Battleship.Board: Login failed";
                    await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                    return StatusCode(StatusCodes.Status204NoContent);
                }

                // publish the message to the GamePlay Queue
                result.SessionToken = generateToken(result.SessionGuid);
                await this.messagePublisher.PublishMessageAsync(JsonConvert.SerializeObject(result), "GamePlay");

                // convert the guid to a string
                result.SessionToken = generateToken(result.SessionGuid);

                // convert the session token
                var token = JsonConvert.SerializeObject(result);
                return Ok(token);
            }
            catch (Exception e)
            {
                var message = $"Battleship.Player: {e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CreatePlayer")]
        public async Task<ActionResult> CreatePlayer([FromBody] Player player)
        {
            try
            {
                var result = await this.playerRepository.CreatePlayer(player);
                if (result == Guid.Empty) return StatusCode(StatusCodes.Status400BadRequest);

                var token = generateToken(result);
                player.SessionToken = token;

                var data = JsonConvert.SerializeObject(player);

                return Ok(data);
            }
            catch (Exception e)
            {
                var message = $"Battleship.Player: {e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetDemoPlayers")]
        public async Task<ActionResult> GetDemoPlayers()
        {
            try
            {
                var result = await this.playerRepository.GetDemoPlayers();
                if (result == null) return BadRequest(false);

                var token = JsonConvert.SerializeObject(result);
                return Ok(token);
            }
            catch (Exception e)
            {
                var message = $"Battleship.Player: {e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
            }

            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpGet]
        [Route("DemoLogin")]
        public async Task<ActionResult> DemoLogin(Guid playerId)
        {
            try
            {
                if (playerId == Guid.Empty) return StatusCode(StatusCodes.Status403Forbidden);

                // session is created in the database
                Player result = await this.playerRepository.DemoLogin(playerId);
                if (result == null) return BadRequest(false);

                // publish the message to the GamePlay Queue
                result.SessionToken = generateToken(result.SessionGuid);
                var serializedPlayer = JsonConvert.SerializeObject(result);
                await this.messagePublisher.PublishMessageAsync(serializedPlayer, "GamePlay");

                return Ok(serializedPlayer);
            }
            catch (Exception e)
            {
                var message = $"Battleship.Player: {e.Message}{Environment.NewLine}{e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("PlayerLogout")]
        public async Task<ActionResult> PlayerLogout([FromBody] Player player)
        {
            try
            {
                var result = await this.playerRepository.PlayerLogout(player.PlayerId);
                if (!result)
                {
                    var message = $"Logout failed {player?.Firstname} {player?.Lastname}";
                    await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok();
            }
            catch (Exception e)
            {
                var message = $"Battleship.Board: {e.Message}{Environment.NewLine}{e.StackTrace}";
                await this.messagePublisher.PublishMessageAsync(message, "AuditLog");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public string Get()
        {
            return "Player API started.";
        }

        private string generateToken(Guid result)
        {
            var sessionBytes = Encoding.UTF8.GetBytes(result.ToString());
            return Convert.ToBase64String(sessionBytes);
        }
    }
}