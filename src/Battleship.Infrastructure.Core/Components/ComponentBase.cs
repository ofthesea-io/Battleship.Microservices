namespace Battleship.Infrastructure.Core.Components
{
    public abstract class ComponentBase
    {
        #region Fields

        protected const char Hit = '*';

        protected const char Miss = 'M';

        protected const char Water = '~';

        protected const char DestroyerCode = 'D';

        protected const char BattleShipCode = 'B';

        protected static readonly object SyncObject = new object();

        protected readonly int GridDimension = 12;

        protected readonly int Index = 1;

        protected readonly int XInitialPoint = 65;

        #endregion
    }
}