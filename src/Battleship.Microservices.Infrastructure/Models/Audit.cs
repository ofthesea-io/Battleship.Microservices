namespace Battleship.Microservices.Infrastructure.Models
{
    using System;

    public class Audit
    {
        #region Properties

        public int AuditId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        #endregion
    }
}