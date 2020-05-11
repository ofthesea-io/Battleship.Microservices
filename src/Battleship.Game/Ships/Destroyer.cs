namespace Battleship.Game.Ships
{
    using Battleship.Microservices.Core.Components;

    public sealed class Destroyer : ComponentBase, IShip
    {
        #region Fields

        private readonly int shipLength = 4;

        private readonly char shipType = ComponentBase.DestroyerCode;

        private int shipHit;

        #endregion

        #region Constructors

        public Destroyer(int shipIndex)
        {
            this.ShipLength = this.shipLength;
            this.ShipCode = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #endregion

        #region IShip Members

        public int ShipLength { get; }

        public char ShipCode { get; }

        public sbyte ShipSegmentHit { get; set; }

        public int ShipIndex { get; }

        #endregion IShip Members
    }
}