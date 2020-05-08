namespace Battleship.Audit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Battleship.Microservices.Infrastructure.Models;
    using Battleship.Microservices.Infrastructure.Repository;
    using Battleship.Microservices.Infrastructure.Utilities;

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
                { "Message", message },
                { "Username", username }
            };

            return this.ExecuteAsync(parameters);
        }

        #endregion
    }
}