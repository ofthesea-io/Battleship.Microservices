namespace Battleship.Game.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Models;
    using Battleship.Microservices.Infrastructure.Repository;
    using Newtonsoft.Json;
    using Ships;
    using Utilities;

    public class GameRepository : RepositoryCore, IGameRepository
    {
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        private readonly IShipRandomiser shipRandomiser;

        public GameRepository(string database) : base(database)
        {
            this.shipRandomiser = ShipRandomiser.Instance();
        }

        public Task<bool> UserInput(Coordinate coordinate, string sessionToken)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateShipCoordinates(string updateShipCoordinates, string sessionToken)
        {
            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"updateShipCoordinates", updateShipCoordinates}
            };

            await ExecuteAsync(parameters);
        }

        public async Task<string> GetShipCoordinates(string sessionToken)
        {
            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken}
            };

            return await ExecuteScalarAsync<string>(parameters);
        }

        public async Task<string> StartGame(string sessionToken, int numberOfShips)
        {
            if (string.IsNullOrEmpty(sessionToken) || numberOfShips == 0) throw new ArgumentException();

            var getRandomShips = BattleshipExtensions.GetRandomShips(numberOfShips);
            var ships = this.shipRandomiser.GetRandomisedShipCoordinates(getRandomShips);

            var shipCoordinates =
                JsonConvert.SerializeObject(ships.ToArray(), Formatting.Indented, this.jsonSerializerSettings);

            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"shipCoordinates", shipCoordinates}
            };

            return await ExecuteScalarAsync<string>(parameters);
        }

        public async Task<bool> CreatePlayer(string sessionToken, Guid playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"playerId", playerId}
            };

            return await ExecuteScalarAsync<bool>(parameters);
        }

        public bool CheckPlayerStatus(string sessionToken)
        {
            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken}
            };

            return ExecuteScalar<bool>(parameters);
        }
    }
}