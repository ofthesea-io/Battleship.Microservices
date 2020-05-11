namespace Battleship.Warehouse.Infrastructure
{
    using System.Collections.Generic;

    using Battleship.Microservices.Core.Repository;
    using Battleship.Warehouse.Model;

    public class WareHouseRepository : RepositoryCore, IWareHouseRepository
    {
        #region Constructors

        public WareHouseRepository(string databaseName)
            : base(databaseName)
        {
        }

        #endregion

        #region Methods

        public IEnumerable<Player> GetTopTenPlayers()
        {
            return this.Execute<Player>();
        }

        #endregion
    }
}