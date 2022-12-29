using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RUIAN.Types
{
    public class RuianParcelResponse
    {
        [JsonPropertyName("type")]
        public string Typ { get; set; }

        [JsonPropertyName("features")]
        public List<RuianParcel> Parcely { get; set; }
    }
}