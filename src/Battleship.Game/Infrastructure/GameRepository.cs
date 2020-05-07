namespace Battleship.Game.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microservices.Infrastructure.Models;
    using Microservices.Infrastructure.Repository;
    using Models;
    using Newtonsoft.Json;
    using Ships;
    using Utilities;

    public class GameRepository : RepositoryCore, IGameRepository
    {
        #region Fields

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        private readonly IShipRandomiser shipRandomiser;

        #endregion

        #region Constructors

        public GameRepository(string database) : base(database)
        {
            this.shipRandomiser = ShipRandomiser.Instance();
        }

        #endregion

        #region Methods

        public Task<bool> UserInput(Coordinate coordinate, string sessionToken)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateShipCoordinates(string updateShipCoordinates, string sessionToken)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"updateShipCoordinates", updateShipCoordinates}
            };

            await this.ExecuteAsync(parameters);
        }

        public async Task<string> GetShipCoordinates(string sessionToken)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken}
            };

            return await this.ExecuteScalarAsync<string>(parameters);
        }

        public async Task<string> StartGame(string sessionToken, int numberOfShips)
        {
            if (string.IsNullOrEmpty(sessionToken) || numberOfShips == 0) throw new ArgumentException();

            List<IShip> getRandomShips = BattleshipExtensions.GetRandomShips(numberOfShips);
            SortedDictionary<Coordinate, Segment> ships = this.shipRandomiser.GetRandomisedShipCoordinates(getRandomShips);

            var shipCoordinates =
                JsonConvert.SerializeObject(ships.ToArray(), Formatting.Indented, this.jsonSerializerSettings);

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"shipCoordinates", shipCoordinates}
            };

            return await this.ExecuteScalarAsync<string>(parameters);
        }

        public async Task<bool> CreatePlayer(string sessionToken, Guid playerId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"playerId", playerId}
            };

            return await this.ExecuteScalarAsync<bool>(parameters);
        }

        public bool CheckPlayerStatus(string sessionToken)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken}
            };

            return this.ExecuteScalar<bool>(parameters);
        }

        #endregion
    }
}