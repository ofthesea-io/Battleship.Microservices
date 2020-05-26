namespace Battleship.Microservices.Core.Models
{
    using System;

    using Newtonsoft.Json;

    public class Player
    {
        #region Properties

        [JsonProperty("playerId")]
        public Guid PlayerId { get; set; }

        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; }

        [JsonProperty("shipCoordinates")]
        public string ShipCoordinates { get; set; }

        [JsonProperty("firstName")]
        public string Firstname { get; set; }

        [JsonProperty("lastName")]
        public string Lastname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("scoreCard")]
        public ScoreCard ScoreCard { get; set; }

        [JsonProperty("isDemoPlayer")]
        public bool IsDemoPlayer { get; set; }

        #endregion
    }
}