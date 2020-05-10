namespace Battleship.Audit.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Models;
    using Battleship.Microservices.Core.Utilities;

    public interface IAuditRepository
    {
        #region Methods

        Task<IEnumerable<Audit>> GetAuditMessages();

        Task<IEnumerable<Audit>> GetAuditMessagesByAuditType(AuditType auditType);

        Task SaveAuditMessage(AuditType auditType, string message, string username);

        #endregion
    }
}