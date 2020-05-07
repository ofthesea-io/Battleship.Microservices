namespace Battleship.Audit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Microservices.Infrastructure.Models;
    using Microservices.Infrastructure.Repository;
    using Microservices.Infrastructure.Utilities;

    public class AuditRepository : RepositoryCore, IAuditRepository
    {
        public AuditRepository(string database) : base(database)
        {
        }

        #region Methods

        public IEnumerable<Audit> GetAuditMessages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Audit> GetAuditMessagesByAuditType(AuditType auditType)
        {
            throw new NotImplementedException();
        }

        public void SaveAuditMessage(AuditType auditType, string content)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}