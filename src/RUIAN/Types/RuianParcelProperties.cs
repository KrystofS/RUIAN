using System.Text.Json.Serialization;

namespace RUIAN.Types
{
    /// <summary>
    /// This class corresponds to properties of the response from RUIAN API.
    /// </summary>
    public class RuianParcelProperties
    {
        [JsonPropertyName("objectid")]
        public int ObjectId { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("nespravny")]
        public object Nespravny { get; set; }

        [JsonPropertyName("kmenovecislo")]
        public int KmenoveCislo { get; set; }

        [JsonPropertyName("poddelenicisla")]
        public int? PoddeleniCisla { get; set; }

        [JsonPropertyName("vymeraparcely")]
        public int VymeraParcely { get; set; }

        [JsonPropertyName("zpusobyvyuzitipozemku")]
        public UseTypes? ZpusobVvyuzitiPozemku { get; set; } = UseTypes.Nespecifikovano;

        [JsonPropertyName("druhcislovanikod")]
        public NumberingType? DruhCislovaniKod { get; set; } = NumberingType.Nespecifikovana;

        [JsonPropertyName("druhpozemkukod")]
        public int DruhPozemkuKOd { get; set; }

        [JsonPropertyName("katastralniuzemi")]
        public int KatastralniUzemi { get; set; }

        [JsonPropertyName("platiod")]
        public long PlatiOd { get; set; }

        [JsonPropertyName("platido")]
        public long? PlatiDo { get; set; }

        [JsonPropertyName("idtransakce")]
        public int IdTransakce { get; set; }

        [JsonPropertyName("rizeniid")]
        public long RizeniId { get; set; }

        [JsonPropertyName("cisloparcely")]
        public string CisloParcely { get; set; }

        [JsonPropertyName("katastralniuzemicisloparcely")]
        public string KatastralniUzemiCisloParcely { get; set; }
    }
}