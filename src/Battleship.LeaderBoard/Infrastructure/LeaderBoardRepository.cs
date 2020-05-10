namespace Battleship.LeaderBoard.Infrastructure
{
    using Battleship.Microservices.Core.Repository;

    public class LeaderBoardRepository : RepositoryCore, ILeaderBoardRepository
    {
        #region Constructors

        public LeaderBoardRepository(string database) : base(database)
        {
        }

        #endregion
    }
}