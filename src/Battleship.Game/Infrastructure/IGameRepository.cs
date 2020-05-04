namespace Battleship.Game.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Models;

    public interface IGameRepository
    {
        Task<bool> UserInput(Coordinate coordinate, string sessionToken);

        Task UpdateShipCoordinates(string updateShipCoordinates, string sessionToken);

        Task<string> GetShipCoordinates(string sessionToken);

        Task<string> StartGame(string sessionToken, int numberOfShips);

        Task<bool> CreatePlayer(string sessionToken, Guid player);

        bool CheckPlayerStatus(string sessionToken);
    }
}