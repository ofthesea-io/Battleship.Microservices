namespace Battleship.Audit.Infrastructure
{
    using Microservices.Infrastructure.Repository;

    public class AuditRepository : RepositoryCore, IAuditRepository
    {
        public AuditRepository(string database) : base(database)
        {
        }
    }
}