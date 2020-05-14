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

        public async Task<IEnumerable<Audit>> GetAuditContent()
        {
            return await this.ExecuteAsync<Audit>();
        }

        public async Task<IEnumerable<Audit>> GetAuditContentByAuditTypeHourRange(AuditType auditType, int hours = 0)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "AuditTypeId", (int)auditType }, { "Hours", hours } };

            return await this.ExecuteAsync<Audit>(parameters);
        }

        public Task SaveAuditContent(string content, AuditType auditType, DateTime timestamp)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "AuditTypeId", auditType }, { "Content", content }, { "Timestamp", timestamp } };

            return this.ExecuteAsync(parameters);
        }

        #endregion
    }
}