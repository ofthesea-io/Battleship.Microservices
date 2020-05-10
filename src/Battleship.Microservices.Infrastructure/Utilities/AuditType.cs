namespace Battleship.Microservices.Core.Utilities
{
    public enum AuditType
    {
        /// <summary>
        /// An exception or Error within the application
        /// </summary>
        Error = 1,

        /// <summary>
        /// Log errors
        /// </summary>
        Log = 2,

        /// <summary>
        /// Content to save
        /// </summary>
        Content = 3
    }
}