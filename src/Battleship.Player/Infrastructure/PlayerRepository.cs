namespace Battleship.Player.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Repository;
    using Models;

    public class PlayerRepository : RepositoryCore, IPlayerRepository
    {
        public PlayerRepository(string database) : base(database)
        {
        }

        public async Task<Guid> CreatePlayer(Player player)
        {
            var parameters = new Dictionary<string, object>
            {
                {"firstname", player.Firstname},
                {"lastname", player.Lastname},
                {"email", player.Email},
                {"password", player.Password}
            };

            return await ExecuteScalarAsync<Guid>(parameters);
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            return await ExecuteAsync<Player>();
        }

        public async Task<IEnumerable<Player>> GetDemoPlayers()
        {
            return await ExecuteAsync<Player>();
        }

        public async Task<Player> GetPlayer(Guid playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"PlayerId", playerId}
            };

            return await ExecuteScalarAsync<Player>(parameters);
        }

        public async Task<bool> PlayerLogout(Guid playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"PlayerId", playerId}
            };

            return await ExecuteScalarAsync<bool>(parameters);
        }

        public async Task<Player> PlayerLogin(Player player)
        {
            var parameters = new Dictionary<string, object>
            {
                {"Email", player.Email},
                {"Password", player.Password}
            };

            return await ExecuteScalarAsync<Player>(parameters);
        }

        public async Task<Player> DemoLogin(Guid playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"PlayerId", playerId}
            };

            return await ExecuteScalarAsync<Player>(parameters);
        }
    }
}