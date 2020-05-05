namespace Battleship.Game.Ships
{
    using Microservices.Infrastructure.Components;

    public sealed class Destroyer : ComponentBase, IShip
    {
        private readonly int shipLength = 4;

        private readonly char shipType = DestroyerCode;

        private int shipHit;

        public Destroyer(int shipIndex)
        {
            this.ShipLength = this.shipLength;
            this.ShipChar = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #region IShip Members

        public int ShipLength { get; }

        public char ShipChar { get; }

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

        public int CoordinateStatus { get; set; }

        #endregion IShip Members
    }
}