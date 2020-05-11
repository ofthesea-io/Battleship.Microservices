namespace Battleship.Player.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Battleship.Player.Models;

    using Microsoft.AspNetCore.Mvc;

    public interface IPlayerController
    {
        #region Methods

        Task<ActionResult> CreatePlayer([FromBody] Player player);

        Task<ActionResult> GetDemoPlayers();

        Task<ActionResult> DemoLogin(Guid playerId);

        Task<ActionResult> PlayerLogin([FromBody] Player player);

        #endregion
    }
}