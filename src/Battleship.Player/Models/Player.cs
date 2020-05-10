namespace Battleship.Player.Models
{
    using System;

    using Battleship.Microservices.Core.Models;

    using Newtonsoft.Json;

    public class Player
    {
        #region Properties

        [JsonProperty("playerId")] public Guid PlayerId { get; set; }

        [JsonProperty("firstName")] public string Firstname { get; set; }

        [JsonProperty("lastName")] public string Lastname { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("password")] public string Password { get; set; }

        [JsonProperty("sessionToken")] public string SessionToken { get; set; }

        [JsonProperty("scoreCard")] public ScoreCard ScoreCard { get; set; }

        [JsonIgnore] public Guid SessionGuid { get; set; }

        #endregion
    }
}