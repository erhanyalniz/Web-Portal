using System.Text.Json.Serialization;

namespace BlazorWebApp.Models
{
    public class Order
    {
        [JsonPropertyName("OrderNo")]
        public string OrderNr { get; set; }

        [JsonPropertyName("Units")]
        public Unit[] Units { get; set; }
    }
}
