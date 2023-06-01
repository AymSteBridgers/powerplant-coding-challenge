using SPaaS.Api.Models;

namespace SPaaS.Api.Services
{
    public interface IProductionPlanService
    {
        ProductionPlanResult ComputeProductionPlan(ProductionPlanRequest request);
    }
}
