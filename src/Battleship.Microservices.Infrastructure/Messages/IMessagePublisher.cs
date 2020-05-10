namespace Battleship.Microservices.Core.Messages
{
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Utilities;

    public interface IMessagePublisher
    {
        #region Properties

        string Host { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        string Exchange { get; set; }

        string Queue { get; set; }

        #endregion

        #region Methods

        Task PublishMessageAsync(string message, string queue);

        Task PublishAuditLogMessageAsync(AuditType auditType, string message);

        #endregion
    }
}