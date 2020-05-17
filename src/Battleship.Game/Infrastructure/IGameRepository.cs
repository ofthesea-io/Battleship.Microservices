namespace Battleship.Game.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Models;

    public interface IGameRepository
    {
        #region Methods

        Task<bool> UserInput(Coordinate coordinate, string sessionToken);

        Task UpdateShipCoordinates(string updateShipCoordinates, string sessionToken);

        Task<string> GetShipCoordinates(string sessionToken);

        Task<string> StartGame(string sessionToken, int numberOfShips);

        Task<bool> CreatePlayer(string sessionToken, Guid player);

        bool CheckPlayerStatus(string sessionToken);

        Task<bool> SetGameCompleted(string sessionToken);

        #endregion
    }
}