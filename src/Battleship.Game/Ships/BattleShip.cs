namespace Battleship.Game.Ships
{
    using Battleship.Microservices.Core.Components;

    public sealed class BattleShip : ComponentBase, IShip
    {
        #region Fields

        private readonly int shipLength = 5;

        private readonly char shipType = ComponentBase.BattleShipCode;

        private int shipHit;

        #endregion

        #region Constructors

        public BattleShip(int shipIndex)
        {
            this.ShipLength = this.shipLength;
            this.ShipChar = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #endregion

        #region IShip Members

        public int ShipHit
        {
            get => this.shipHit;
            set
            {
                if (this.shipHit == this.ShipLength - this.Index)
                    this.IsShipSunk = true;
                else
                    this.shipHit++;
            }
        }

        public int ShipIndex { get; }

        public bool IsShipSunk { get; set; }

        public int ShipLength { get; }

        public char ShipChar { get; }

        #endregion IShip Members
    }
}