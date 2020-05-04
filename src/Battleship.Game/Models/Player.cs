namespace Battleship.Game.Models
{
    using System;
    using Newtonsoft.Json;

    public class Player
    {
        [JsonProperty("playerId")] public Guid PlayerId { get; set; }

        [JsonProperty("sessionToken")] public string SessionToken { get; set; }

        [JsonProperty("shipCoordinates")] public string ShipCoordinates { get; set; }
    }
}