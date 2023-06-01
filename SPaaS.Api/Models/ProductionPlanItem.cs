using System.Text.Json.Serialization;

namespace SPaaS.Api.Models
{
    public class ProductionPlanItem
    {
        [JsonPropertyName("name")]
        public required string PowerPlantName { get; set; }

        [JsonPropertyName("p")]
        public decimal LoadToProduce { get; set; }
    }
}
