namespace SPaaS.Api.Exceptions
{
    public class InvalidLoadException : ProductionPlanException
    {
        public InvalidLoadException(string message) : base(message)
        {
        }
    }
}
