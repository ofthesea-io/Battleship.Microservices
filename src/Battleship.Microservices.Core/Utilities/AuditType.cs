namespace Battleship.Microservices.Core.Utilities
{
    public enum AuditType
    {
        /// <summary>
        ///     An exception or Error within the application
        /// </summary>
        Error = 1,

        /// <summary>
        ///     Warning errors
        /// </summary>
        Warning = 2,

        /// <summary>
        ///     Info to save
        /// </summary>
        Info = 3
    }
}