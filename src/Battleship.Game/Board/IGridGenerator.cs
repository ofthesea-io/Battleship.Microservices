namespace Battleship.Game.Board
{
    /// <summary>
    ///     The gaming board for battle ships
    /// </summary>
    public interface IGridGenerator
    {
        #region Properties

        int? NumberOfSegments { get; set; }

        int? NumberOfOccupiedSegments { get; set; }

        #endregion

        #region Methods

        int[] GetNumericRows();

        string[] GetAlphaColumnChars();

        #endregion
    }
}