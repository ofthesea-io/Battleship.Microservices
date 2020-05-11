namespace Battleship.Player.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Player.Models;

    public interface IPlayerRepository
    {
        #region Methods

        Task<Guid> CreatePlayer(Player player);

        Task<IEnumerable<Player>> GetPlayers();

        Task<IEnumerable<Player>> GetDemoPlayers();

        Task<Player> GetPlayer(Guid playerId);

        Task<Player> PlayerLogin(Player player);

        Task<bool> PlayerLogout(Guid playerId);

        Task<Player> DemoLogin(Guid playerId);

        Task<bool> IsAuthenticated(string auth);

        #endregion
    }
}