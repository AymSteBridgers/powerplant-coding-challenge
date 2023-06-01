using System.Text.Json.Serialization;

namespace SPaaS.Api.Models
{
    public class FuelsInfo
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal GasCost { get; set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal KerosineCost { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public decimal CarbonConsumptionCost { get; set; }

        [JsonPropertyName("wind(%)")]
        public decimal WindPercentage { get; set; }
    }
}
