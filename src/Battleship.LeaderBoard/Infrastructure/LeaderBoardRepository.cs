namespace Battleship.LeaderBoard.Infrastructure
{
    using Microservices.Infrastructure.Repository;

    public class LeaderBoardRepository : RepositoryCore, ILeaderBoardRepository
    {
        #region Constructors

        public LeaderBoardRepository(string database) : base(database)
        {
        }

        #endregion
    }
}