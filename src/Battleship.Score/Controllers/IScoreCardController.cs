namespace Battleship.Score.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public interface IScoreCardController
    {
        Task<ActionResult> GetPlayerScoreCard();
    }
}