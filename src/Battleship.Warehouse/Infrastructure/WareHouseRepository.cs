namespace Battleship.Warehouse.Infrastructure
{
    using System.Collections.Generic;
    using Battleship.Microservices.Infrastructure.Repository;
    using Model;

    public class WareHouseRepository : RepositoryCore, IWareHouseRepository
    {
        public WareHouseRepository(string databaseName) : base(databaseName)
        {
        }

        public IEnumerable<Player> GetTopTenPlayers()
        {
            return Execute<Player>();
        }
    }
}