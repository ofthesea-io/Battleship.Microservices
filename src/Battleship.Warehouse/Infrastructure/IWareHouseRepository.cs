namespace Battleship.Warehouse.Infrastructure
{
    using System.Collections.Generic;
    using Model;

    public interface IWareHouseRepository
    {
        IEnumerable<Player> GetTopTenPlayers();
    }
}