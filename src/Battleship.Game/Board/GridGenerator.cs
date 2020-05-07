namespace Battleship.Game.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microservices.Infrastructure.Components;
    using Microservices.Infrastructure.Models;
    using Models;
    using Ships;

    public class GridGenerator : ComponentBase, IGridGenerator
    {
        #region Fields

        private static volatile GridGenerator instance;

        private readonly ISegmentation segmentation;

        private readonly IShipRandomiser shipRandomiser;

        private readonly List<IShip> ships;

        private int boardLeft;

        private int boardTop;

        #endregion

        #region Constructors

        protected GridGenerator(
            ISegmentation segmentation,
            IShipRandomiser shipRandomiser,
            List<IShip> ships)
        {
            this.segmentation = segmentation;
            this.shipRandomiser = shipRandomiser;
            this.ships = ships;
        }

        #endregion

        #region Properties

        public int? NumberOfSegments { get; set; }

        public int? NumberOfOccupiedSegments { get; set; }

        public int TotalSegments => this.GetAlphaColumnChars().Length * this.GetNumericRows().Length;

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the x dimension column
        /// </summary>
        /// <returns>Array of int</returns>
        public int[] GetNumericRows()
        {
            int[] row = new int[this.GridDimension];
            var counter = 0;
            for (var i = 1; i <= this.GridDimension; i++)
            {
                row[counter] = i;
                counter++;
            }

            return row;
        }

        /// <summary>
        ///     Gets the y dimension column
        /// </summary>
        /// <returns>Array of string</returns>
        public string[] GetAlphaColumnChars()
        {
            var xDimention = this.XInitialPoint + this.GridDimension;
            string[] column = new string[this.GridDimension];
            var counter = 0;
            for (var i = this.XInitialPoint; i < xDimention; i++)
            {
                column[counter] = ((char) i).ToString();
                counter++;
            }

            return column;
        }

        public static GridGenerator Instance()
        {
            if (GridGenerator.instance == null)
                lock (ComponentBase.SyncObject)
                {
                    if (GridGenerator.instance == null)
                    {
                        ISegmentation segmentation = Segmentation.Instance();
                        IShipRandomiser shipRandomiser = ShipRandomiser.Instance();
                        List<IShip> ships = new List<IShip>();
                        GridGenerator.instance = new GridGenerator(segmentation, shipRandomiser, ships);
                    }
                }

            return GridGenerator.instance;
        }

        private void CreateSegmentationGrid()
        {
            var yCounter = 1;
            while (yCounter <= this.GridDimension)
            {
                for (var xCounter = 0; xCounter <= this.GridDimension - this.Index; xCounter++)
                {
                    var segment = new Segment(ComponentBase.Water);
                    var coordinates = new Coordinate(this.XInitialPoint + xCounter, yCounter);
                    this.segmentation.AddSegment(coordinates, segment);
                }

                this.boardTop++;
                yCounter++;

                Console.SetCursorPosition(this.boardLeft, this.boardTop);
            }

            this.NumberOfSegments = this.segmentation.GetSegments().Count();

            // update the board with randomly generated ship coordinates
            this.UpdateSegmentationGridWithShips();
        }

        private void UpdateSegmentationGridWithShips()
        {
            SortedDictionary<Coordinate, Segment> segments = this.shipRandomiser.GetRandomisedShipCoordinates(this.ships);

            this.segmentation.UpdateSegmentRange(segments);

            this.NumberOfOccupiedSegments = this.segmentation.GetSegments().Count(q => !q.Value.IsEmpty);
        }

        #endregion
    }
}