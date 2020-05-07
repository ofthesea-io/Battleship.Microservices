namespace Battleship.Score.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public interface IScoreCardController
    {
        #region Methods

        Task<ActionResult> GetPlayerScoreCard();

        #endregion
    }
}