namespace Battleship.Game.Utilities
{
    using System;
    using System.Collections.Generic;

    using Battleship.Infrastructure.Core.Models;

    public class CoordinateComparer : IComparer<Coordinate>
    {
        #region Methods

        #region IComparer<Coordinate> Members

        public int Compare(Coordinate one, Coordinate two)
        {
            if (one == null || two == null) throw new NullReferenceException();

            // first by X
            int result = one.X.CompareTo(two.X);

            // then By Y
            if (result == 0) result = one.Y.CompareTo(two.Y);

            return result;
        }

        #endregion IComparer<Coordinate> Members

        #endregion
    }
}