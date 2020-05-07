namespace Battleship.Game.Models
{
    using Microservices.Infrastructure.Models;

    public class PlayerCommand
    {
        #region Properties

        public Coordinate Coordinate { get; set; }

        public ScoreCard ScoreCard { get; set; }

        #endregion
    }
}