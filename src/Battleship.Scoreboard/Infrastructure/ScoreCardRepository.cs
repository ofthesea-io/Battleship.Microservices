namespace Battleship.Scoreboard.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Infrastructure.Core.Repository;
    using Battleship.Score.Infrastructure;

    public class ScoreCardRepository : RepositoryCore, IScoreCardRepository
    {
        #region Constructors

        public ScoreCardRepository(string databaseName)
            : base(databaseName)
        {
        }

        #endregion

        #region Methods

        public async Task<string> ManagePlayerScoreCard(string sessionToken, string scoreCard)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "sessionToken", sessionToken }, { "scoreCard", scoreCard } };

            return await this.ExecuteScalarAsync<string>(parameters);
        }

        public async Task<string> GetPlayerScoreCard(string sessionToken)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "sessionToken", sessionToken } };

            return await this.ExecuteScalarAsync<string>(parameters);
        }

        #endregion
    }
}