namespace Battleship.Warehouse.Infrastructure
{
    using System.Collections.Generic;
    using Microservices.Infrastructure.Repository;
    using Model;

    public class WareHouseRepository : RepositoryCore, IWareHouseRepository
    {
        #region Constructors

        public WareHouseRepository(string databaseName) : base(databaseName)
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