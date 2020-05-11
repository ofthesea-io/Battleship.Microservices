namespace Battleship.Game.Board
{
    using Battleship.Microservices.Core.Components;

    public class GridGenerator : ComponentBase, IGridGenerator
    {
        #region Fields

        private static volatile GridGenerator instance;

        #endregion

        #region Constructors

        protected GridGenerator()
        {
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
        /// <returns>Array of numbers</returns>
        public int[] GetNumericRows()
        {
            int[] row = new int[this.GridDimension];
            int counter = 0;
            for (int i = 1; i <= this.GridDimension; i++)
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
            int xDimension = this.XInitialPoint + this.GridDimension;
            string[] column = new string[this.GridDimension];
            int counter = 0;
            for (int i = this.XInitialPoint; i < xDimension; i++)
            {
                column[counter] = ((char)i).ToString();
                counter++;
            }

            return column;
        }

        /// <summary>
        ///     Creates a singleton object that is thread safe
        /// </summary>
        /// <returns>Returns a single instance o GridGenerator</returns>
        public static GridGenerator Instance()
        {
            if (GridGenerator.instance == null)
                lock (ComponentBase.SyncObject)
                {
                    if (GridGenerator.instance == null) GridGenerator.instance = new GridGenerator();
                }

            return GridGenerator.instance;
        }

        #endregion
    }
}