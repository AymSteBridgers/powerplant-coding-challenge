namespace SPaaS.Api.Exceptions
{
    public class InvalidPowerPlantException : ProductionPlanException
    {
        public InvalidPowerPlantException(string message) : base (message)
        {            
        }
    }
}
