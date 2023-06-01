namespace SPaaS.Api.Models
{
    public class ProductionPlanResult : List<ProductionPlanItem>
    {
        public ProductionPlanResult()
        {
        }

        public ProductionPlanResult(IEnumerable<ProductionPlanItem> list) : base(list)
        {
        }
    }
}
