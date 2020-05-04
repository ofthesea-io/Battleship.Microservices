namespace Battleship.Game.Models
{
    using Battleship.Microservices.Infrastructure.Models;

    public class PlayerCommand
    {
        public Coordinate Coordinate { get; set; }

        public ScoreCard ScoreCard { get; set; }
    }
}