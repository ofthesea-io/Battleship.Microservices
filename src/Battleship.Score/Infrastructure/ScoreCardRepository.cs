namespace Battleship.Score.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Battleship.Microservices.Infrastructure.Repository;

    public class ScoreCardRepository : RepositoryCore, IScoreCardRepository
    {
        public ScoreCardRepository(string databaseName) : base(databaseName)
        {
        }

        public async Task<string> ManagePlayerScoreCard(string sessionToken, string scoreCard)
        {
            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken},
                {"scoreCard", scoreCard}
            };

            return await ExecuteScalarAsync<string>(parameters);
        }

        public async Task<string> GetPlayerScoreCard(string sessionToken)
        {
            var parameters = new Dictionary<string, object>
            {
                {"sessionToken", sessionToken}
            };

            return await ExecuteScalarAsync<string>(parameters);
        }
    }
}