namespace Battleship.Score.Infrastructure
{
    using System.Threading.Tasks;

    public interface IScoreCardRepository
    {
        Task<string> ManagePlayerScoreCard(string sessionToken, string scoreCard);

        Task<string> GetPlayerScoreCard(string sessionToken);
    }
}