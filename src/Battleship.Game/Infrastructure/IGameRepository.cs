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

        string GetShipCoordinates(string sessionToken);

        Task<int> StartGame(string sessionToken, int numberOfShips);

        Task<bool> CreatePlayer(string sessionToken, Guid player);

        Guid CheckPlayerStatus(string sessionToken);

        Task<bool> SetGameCompleted(string sessionToken);

        #endregion
    }
}