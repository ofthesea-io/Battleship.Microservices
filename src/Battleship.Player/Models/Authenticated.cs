namespace Battleship.Player.Models
{
    using Newtonsoft.Json;

    public class Authenticated
    {
        [JsonProperty("isDemo")]
        public byte IsDemo { get; set; }

        [JsonProperty("isAdmin")]
        public byte IsAdmin { get; set; }

        [JsonProperty("level")]
        public byte Level { get; set; }
    }
}