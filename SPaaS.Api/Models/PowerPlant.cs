using SPaaS.Api.Enums;
using System.Text.Json.Serialization;

namespace SPaaS.Api.Models
{
    public class PowerPlant
    {

        public required string Name { get; set; }
        public required string Type { get; set; }
        public decimal Efficiency { get; set; }
        public decimal Pmin { get; set; }
        public decimal Pmax { get; set; }

        [JsonIgnore]
        public PowerPlantType PowerPlantType
        {
            get
            {
                if (Enum.TryParse(Type, ignoreCase: true, out PowerPlantType powerPlantType))
                {
                    return powerPlantType;
                }
                else
                {
                    return PowerPlantType.Undefined;
                }
            }

        }
    }
}
