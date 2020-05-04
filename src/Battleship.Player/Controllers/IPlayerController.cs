namespace Battleship.Player.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public interface IPlayerController
    {
        Task<ActionResult> CreatePlayer([FromBody] Player player);

        Task<ActionResult> GetDemoPlayers();

        Task<ActionResult> DemoLogin(Guid playerId);

        Task<ActionResult> PlayerLogin([FromBody] Player player);
    }
}