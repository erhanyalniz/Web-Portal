using System.Text.Json.Serialization;

namespace BlazorWebApp.Models

{
    public class Unit
    {
        [JsonPropertyName("UnitNummer")]
        public string Nummer { get; set; }

        [JsonPropertyName("Rederij")]
        public string Rederij { get; set; }

        [JsonPropertyName("TypeUnit")]
        public string Type { get; set; }

        [JsonPropertyName("OrderRegelState")]
        public string Status { get; set; }

    }
}
