namespace Battleship.LeaderBoard.Infrastructure
{
    using Battleship.Microservices.Infrastructure.Repository;

    public class LeaderBoardRepository : RepositoryCore, ILeaderBoardRepository
    {
        public LeaderBoardRepository(string database) : base(database)
        {
        }
    }
}