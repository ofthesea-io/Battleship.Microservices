namespace Battleship.Game.Controllers
{
    using System.Threading.Tasks;

    using Battleship.Game.Models;

    using Microsoft.AspNetCore.Mvc;

    public interface IBoardController
    {
        #region Methods

        Task<ActionResult<GamingGrid>> GetGamingGrid();

        Task<ActionResult> UserInput([FromBody] PlayerCommand playerCommand);

        Task<ActionResult> SetGameCompleted();

        Task<ActionResult> StartGame(int numberOfShips);

        #endregion
    }
}