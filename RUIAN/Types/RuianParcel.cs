using System.Text.Json.Serialization;

namespace RUIAN.Types
{
    public class RuianParcel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("geometry")]
        public object Geometry { get; set; }

        [JsonPropertyName("properties")]
        public RuianParcelProperties Properties { get; set; }
    }
}