namespace Battleship.Game.Ships
{
    using Battleship.Microservices.Infrastructure.Components;

    public sealed class BattleShip : ComponentBase, IShip
    {
        private readonly int shipLength = 5;

        private readonly char shipType = BattleShipCode;

        private int shipHit;

        public BattleShip(int shipIndex)
        {
            this.ShipLength = this.shipLength;
            this.ShipChar = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #region IShip Members

        public int ShipHit
        {
            get => shipHit;
            set
            {
                if (shipHit == this.ShipLength - Index)
                {
                    this.IsShipSunk = true;
                }
                else
                {
                    shipHit++;
                }
            }
        }

        public int ShipIndex { get; }

        public bool IsShipSunk { get; set; }

        public int ShipLength { get; }

        public char ShipChar { get; }

        public int CoordinateStatus { get; set; }

        #endregion IShip Members
    }
}