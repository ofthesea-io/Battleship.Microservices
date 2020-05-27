namespace Battleship.Statistics.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public interface IStatisticController
    {
        #region Methods

        Task<ActionResult> GetTopTenPlayers();

        #endregion
    }
}