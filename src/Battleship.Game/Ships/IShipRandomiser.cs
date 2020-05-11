namespace Battleship.Game.Ships
{
    using System.Collections.Generic;

    using Battleship.Game.Models;
    using Battleship.Microservices.Core.Models;

    /// <summary>
    ///     The ShipRandomiser randomises ships on the Grid
    /// </summary>
    public interface IShipRandomiser
    {
        #region Methods

        /// <summary>
        ///     Generates random coordinates for the ships to be updated in the segmentation grid
        /// </summary>
        /// <param name="ships">IEnumerable list of ships of be add</param>
        /// <returns>List of ship coordinates</returns>
        SortedDictionary<Coordinate, Segment> GetRandomisedShipCoordinates(IList<IShip> ships);

        #endregion
    }
}