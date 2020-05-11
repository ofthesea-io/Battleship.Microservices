namespace Battleship.Statistics.Communication
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRpcClient
    {
        #region Methods

        Task<string> CallAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}