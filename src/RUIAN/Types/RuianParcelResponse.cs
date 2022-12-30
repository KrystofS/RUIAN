using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RUIAN.Types
{
    /// <summary>
    /// This class corresponds to response from RUIAN API.
    /// </summary>
    public class RuianParcelResponse
    {
        [JsonPropertyName("type")]
        public string Typ { get; set; }

        [JsonPropertyName("features")]
        public List<RuianParcel> Parcely { get; set; }
    }
}