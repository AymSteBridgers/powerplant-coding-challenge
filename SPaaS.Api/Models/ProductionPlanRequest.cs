using System.Text.Json.Serialization;

namespace SPaaS.Api.Models
{
    public class ProductionPlanRequest
    {
        public decimal Load { get; set; }

        [JsonPropertyName("fuels")]
        public FuelsInfo FuelsInfo { get; set; } = new FuelsInfo();

        public IList<PowerPlant> PowerPlants { get; set; } = new List<PowerPlant>();
    }
}
