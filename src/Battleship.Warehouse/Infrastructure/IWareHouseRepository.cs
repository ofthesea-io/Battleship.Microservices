namespace Battleship.Warehouse.Infrastructure
{
    using System.Collections.Generic;

    using Battleship.Warehouse.Model;

    public interface IWareHouseRepository
    {
        #region Methods

        IEnumerable<Player> GetTopTenPlayers();

        #endregion
    }
}