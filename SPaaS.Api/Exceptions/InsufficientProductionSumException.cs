namespace SPaaS.Api.Exceptions
{
    public class InsufficientProductionSumException : ProductionPlanException
    {
        public override string Message => "The production of the power plants is insufficient for the requested load.";
    }
}
