namespace SPaaS.Api.Exceptions
{
    public class InvalidFuelsException : ProductionPlanException
    {
        public InvalidFuelsException(string message) : base(message)
        {
        }
    }
}
