using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship.Microservices.Infrastructure.Models
{
    public class Audit
    {
        public int AuditId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }
    }
}
