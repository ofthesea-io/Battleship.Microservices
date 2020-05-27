namespace Battleship.Microservices.Core.Models
{
    using System;

    using Newtonsoft.Json;

    public class Statistics
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("winningPercentage")]
        public double WinningPercentage { get; set; }

        [JsonProperty("completedOn")]
        public DateTime CompletedOn { get; set; }

        [JsonProperty("completedGames")]
        public int CompletedGames { get; set; }

        [JsonProperty("scoreCard")]
        public ScoreCard ScoreCard { get; set; }
    }
}