namespace Battleship.Game.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public interface IBoardController
    {
        #region Methods

        Task<ActionResult<GamingGrid>> GetGamingGrid();

        Task<ActionResult> UserInput([FromBody] PlayerCommand playerCommand);

        #endregion
    }
}