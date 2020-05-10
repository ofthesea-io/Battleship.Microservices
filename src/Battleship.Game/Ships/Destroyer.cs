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
            this.ShipChar = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #endregion

        #region IShip Members

        public int ShipLength { get; }

        public char ShipChar { get; }

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

        #endregion IShip Members
    }
}