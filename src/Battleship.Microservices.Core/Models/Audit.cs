namespace Battleship.Microservices.Core.Models
{
    using System;

    using Battleship.Microservices.Core.Utilities;

    using Newtonsoft.Json;

    public class Audit
    {
        #region Properties

        [JsonProperty("auditType", DefaultValueHandling = DefaultValueHandling.Include)]
        public AuditType AuditType { get; set; }

        [JsonProperty("timestamp", DefaultValueHandling = DefaultValueHandling.Include)]
        public DateTime Timestamp { get; set; }

        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Include)]
        public string Content { get; set; }

        #endregion
    }
}