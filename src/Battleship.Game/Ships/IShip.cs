namespace Battleship.Game.Ships
{
    public interface IShip
    {
        #region Properties

        /// <summary>
        ///    Gets the Ship Length
        /// </summary>
        int ShipLength { get; }

        /// <summary>
        ///     Gets the Ship code
        /// </summary>
        char ShipCode { get; }

        /// <summary>
        ///     Gets or sets Ship hit counter
        /// </summary>
        sbyte ShipSegmentHit { get; set; }

        /// <summary>
        ///    Gets the Ship index  
        /// </summary>
        int ShipIndex { get; }

        #endregion
    }
}