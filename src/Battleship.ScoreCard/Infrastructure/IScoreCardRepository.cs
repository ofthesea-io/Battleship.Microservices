namespace Battleship.ScoreCard.Infrastructure
{
    using System.Threading.Tasks;

    public interface IScoreCardRepository
    {
        #region Methods

        Task<string> ManagePlayerScoreCard(string sessionToken, string scoreCard);

        Task<string> GetPlayerScoreCard(string sessionToken);

        #endregion
    }
}