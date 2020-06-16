namespace Battleship.Infrastructure.Core.Models
{
    using Newtonsoft.Json;

    public class ScoreCard
    {
        #region Properties

        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Include)]
        public string Message { get; set; }

        [JsonProperty("hit", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Hit { get; set; }

        [JsonProperty("miss", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Miss { get; set; }

        [JsonProperty("sunk", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Sunk { get; set; }

        [JsonProperty("total", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Total { get; set; }

        [JsonProperty("isCompleted", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsCompleted { get; set; }

        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; }

        [JsonProperty("isHit", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsHit { get; set; }

        #endregion
    }
}