namespace Battleship.Game.Models
{
    using Battleship.Infrastructure.Core.Models;

    public class PlayerCommand
    {
        #region Properties

        public Coordinate Coordinate { get; set; }

        public ScoreCard ScoreCard { get; set; }

        #endregion
    }
}