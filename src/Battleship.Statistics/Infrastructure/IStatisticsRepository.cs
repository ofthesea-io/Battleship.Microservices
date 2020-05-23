namespace Battleship.Statistics.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Models;

    public interface IStatisticsRepository
    {
        Task<IEnumerable<Statistics>> GetTopTenPlayers();

        Task<Statistics> GetPlayerByEmail(string email);

        Task SaveStatistics(Statistics statistics);
    }
}