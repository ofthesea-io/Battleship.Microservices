namespace Battleship.Microservices.Core.Models
{
    using System;

    using Battleship.Microservices.Core.Utilities;

    using Newtonsoft.Json;

    public class Audit
    {
        #region Properties

        [JsonProperty("auditTypeId", DefaultValueHandling = DefaultValueHandling.Include)]
        public AuditType AuditTypeId { get; set; }

        [JsonProperty("timestamp", DefaultValueHandling = DefaultValueHandling.Include)]
        public DateTime Timestamp { get; set; }

        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Include)]
        public string Content { get; set; }

        #endregion
    }
}