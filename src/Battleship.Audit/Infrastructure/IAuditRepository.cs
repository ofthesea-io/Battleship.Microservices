namespace Battleship.Audit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Models;
    using Battleship.Microservices.Core.Utilities;

    public interface IAuditRepository
    {
        #region Methods

        Task<IEnumerable<Audit>> GetAuditContent();

        Task<IEnumerable<Audit>> GetAuditContentByAuditType(AuditType auditType);

        Task SaveAuditContent(string content, AuditType auditType, DateTime timestamp);

        #endregion
    }
}