namespace Battleship.Warehouse.Infrastructure
{
    using System.Collections.Generic;
    using Model;

    public interface IWareHouseRepository
    {
        #region Methods

        IEnumerable<Player> GetTopTenPlayers();

        #endregion
    }
}