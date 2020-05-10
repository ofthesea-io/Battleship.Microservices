namespace Battleship.Audit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Microservices.Core.Models;
    using Battleship.Microservices.Core.Repository;
    using Battleship.Microservices.Core.Utilities;

    public class AuditRepository : RepositoryCore, IAuditRepository
    {
        #region Constructors

        public AuditRepository(string database)
            : base(database)
        {
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Audit>> GetAuditMessages()
        {
            return await this.ExecuteAsync<Audit>();
        }

        public async Task<IEnumerable<Audit>> GetAuditMessagesByAuditType(AuditType auditType)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "AuditTypeId", auditType }
            };

            return await this.ExecuteAsync<Audit>(parameters);
        }

        public Task SaveAuditMessage(AuditType auditType, string message, string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "AuditTypeId", auditType },
                { "Content", message },
                { "Username", username }
            };

            return this.ExecuteAsync(parameters);
        }

        #endregion
    }
}