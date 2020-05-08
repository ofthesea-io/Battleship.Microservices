namespace Battleship.Audit.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microservices.Infrastructure.Models;
    using Microservices.Infrastructure.Utilities;

    public interface IAuditRepository
    {
        #region Methods

        Task<IEnumerable<Audit>> GetAuditMessages();

        Task<IEnumerable<Audit>> GetAuditMessagesByAuditType(AuditType auditType);

        Task SaveAuditMessage(AuditType auditType, string message, string username);

        #endregion
    }
}