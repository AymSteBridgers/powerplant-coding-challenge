namespace SPaaS.Api.Models
{
    public class PowerPlantDetails
    {
        public required string Name { get; set; }
        public decimal EnergyCost { get; set; }
        public decimal MinProduction { get; set; }
        public decimal MaxProduction { get; set; }
    }
}
