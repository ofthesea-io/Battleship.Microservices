namespace Battleship.Game.Models
{
    using Battleship.Game.Enums;
    using Battleship.Game.Ships;

    using Newtonsoft.Json;

    /// <summary>
    ///     Individual segment (cell) in the segmentation list on the grid
    /// </summary>
    public class Segment
    {
        #region Constructors

        public Segment()
        {
            this.IsEmpty = true;
        }

        [JsonConstructor]
        public Segment(ShipDirection shipDirection, IShip ship)
        {
            this.ShipDirection = shipDirection;
            this.Ship = ship;
            this.IsEmpty = false;
        }

        #endregion

        #region Properties

        public bool IsEmpty { get; set; }

        public ShipDirection ShipDirection { get; set; }

        public IShip Ship { get; set; }

        #endregion
    }
}