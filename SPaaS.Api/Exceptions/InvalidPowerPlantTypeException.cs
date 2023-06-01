namespace SPaaS.Api.Exceptions
{
    public class InvalidPowerPlantTypeException : ProductionPlanException
    {
        private readonly string powerPlantType;

        public InvalidPowerPlantTypeException(string powerPlantType)
        {
            this.powerPlantType = powerPlantType;
        }

        public override string Message => $"The power plant type '{powerPlantType}' is invalid.";
    }
}
