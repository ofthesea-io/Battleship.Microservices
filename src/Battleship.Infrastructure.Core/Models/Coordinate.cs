namespace Battleship.Infrastructure.Core.Models
{
    public class Coordinate
    {
        #region Constructors

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Properties

        public int X { get; }

        public int Y { get; }

        #endregion
    }
}