namespace Battleship.Game.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Battleship.Microservices.Infrastructure.Components;
    using Battleship.Microservices.Infrastructure.Models;
    using Models;
    using Ships;

    public class GridGenerator : ComponentBase, IGridGenerator
    {
        private static volatile GridGenerator instance;

        private readonly ISegmentation segmentation;

        private readonly IShipRandomiser shipRandomiser;

        private readonly List<IShip> ships;

        private int boardLeft;

        private int boardTop;

        protected GridGenerator(
            ISegmentation segmentation,
            IShipRandomiser shipRandomiser,
            List<IShip> ships)
        {
            this.segmentation = segmentation;
            this.shipRandomiser = shipRandomiser;
            this.ships = ships;
        }

        #region Properties

        public int? NumberOfSegments { get; set; }

        public int? NumberOfOccupiedSegments { get; set; }

        public int TotalSegments => this.GetAlphaColumnChars().Length * this.GetNumericRows().Length;

        #endregion Properties

        #region Methods

        public static GridGenerator Instance()
        {
            if (instance == null)
                lock (SyncObject)
                {
                    if (instance == null)
                    {
                        ISegmentation segmentation = Segmentation.Instance();
                        IShipRandomiser shipRandomiser = ShipRandomiser.Instance();
                        var ships = new List<IShip>();
                        instance = new GridGenerator(segmentation, shipRandomiser, ships);
                    }
                }

            return instance;
        }

        /// <summary>
        /// Gets the x dimension column
        /// </summary>
        /// <returns>Array of int</returns>
        public int[] GetNumericRows()
        {
            var row = new int[this.GridDimension];
            var counter = 0;
            for (var i = 1; i <= this.GridDimension; i++)
            {
                row[counter] = i;
                counter++;
            }

            return row;
        }

        /// <summary>
        ///  Gets the y dimension column
        /// </summary>
        /// <returns>Array of string</returns>
        public string[] GetAlphaColumnChars()
        {
            var xDimention = this.XInitialPoint + this.GridDimension;
            var column = new string[this.GridDimension];
            var counter = 0;
            for (var i = this.XInitialPoint; i < xDimention; i++)
            {
                column[counter] = ((char) i).ToString();
                counter++;
            }

            return column;
        }

        private void CreateSegmentationGrid()
        {
            var yCounter = 1;
            while (yCounter <= this.GridDimension)
            {
                for (var xCounter = 0; xCounter <= this.GridDimension - this.Index; xCounter++)
                {
                    var segment = new Segment(Water);
                    var coordinates = new Coordinate(this.XInitialPoint + xCounter, yCounter);
                    this.segmentation.AddSegment(coordinates, segment);
                }

                this.boardTop++;
                yCounter++;

                Console.SetCursorPosition(this.boardLeft, this.boardTop);
            }

            this.NumberOfSegments = this.segmentation.GetSegments().Count();

            // update the board with randomly generated ship coordinates
            UpdateSegmentationGridWithShips();
        }

        private void UpdateSegmentationGridWithShips()
        {
            var segments = this.shipRandomiser.GetRandomisedShipCoordinates(this.ships);

            this.segmentation.UpdateSegmentRange(segments);

            this.NumberOfOccupiedSegments = this.segmentation.GetSegments().Count(q => !q.Value.IsEmpty);
        }

        #endregion Methods
    }
}