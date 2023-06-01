namespace SPaaS.Api.Exceptions
{
    public class ProductionPlanException : Exception
    {
        public ProductionPlanException()
        {
        }

        public ProductionPlanException(string message) : base(message)
        {
        }
    }
}
