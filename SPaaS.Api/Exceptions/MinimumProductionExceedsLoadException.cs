namespace SPaaS.Api.Exceptions
{
    public class MinimumProductionExceedsLoadException : ProductionPlanException
    {
        public override string Message => "No power plant can produce a minimum for the requested load.";
    }
}
