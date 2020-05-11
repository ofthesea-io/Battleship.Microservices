namespace Battleship.Game.Ships
{
    using Battleship.Microservices.Core.Components;

    public sealed class BattleShip : ComponentBase, IShip
    {
        #region Fields

        private readonly int shipLength = 5;

        private readonly char shipType = ComponentBase.BattleShipCode;

        #endregion

        #region Constructors

        public BattleShip(int shipIndex)
        {
            this.ShipLength = this.shipLength;
            this.ShipCode = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #endregion

        #region IShip Members

        public sbyte ShipSegmentHit { get; set; }

        public int ShipIndex { get; }

        public int ShipLength { get; }

        public char ShipCode { get; }

        #endregion IShip Members
    }
}