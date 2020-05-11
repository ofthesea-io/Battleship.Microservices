namespace Battleship.Game.Models
{
    using System.Collections.Generic;

    public class GamingGrid
    {
        #region Properties

        public IEnumerable<string> X { get; set; }

        public IEnumerable<int> Y { get; set; }

        #endregion
    }
}