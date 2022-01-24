using System.Text.Json.Serialization;

namespace TaxCalculator.Models
{
    public class FixerResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("rates")]
        public IDictionary<string, decimal>? Rates { get; set; }

        [JsonPropertyName("error")]
        public IDictionary<string, object>? Error { get; set; }
    }
}
