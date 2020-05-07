namespace Battleship.Audit.Infrastructure
{
    using System.Collections.Generic;
    using Microservices.Infrastructure.Models;
    using Microservices.Infrastructure.Utilities;

    internal interface IAuditRepository
    {
        #region Methods

        IEnumerable<Audit> GetAuditMessages();

        IEnumerable<Audit> GetAuditMessagesByAuditType(AuditType auditType);

        void SaveAuditMessage(AuditType auditType, string content);

        #endregion
    }
}