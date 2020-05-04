namespace Battleship.Game.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public interface IBoardController
    {
        Task<ActionResult<GamingGrid>> GetGamingGrid();

        Task<ActionResult> UserInput([FromBody] PlayerCommand playerCommand);
    }
}