using Microsoft.AspNetCore.Mvc;
using SPaaS.Api.Exceptions;
using SPaaS.Api.Models;
using SPaaS.Api.Services;

namespace SPaaS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionPlanController : Controller
    {
        private readonly ILogger<ProductionPlanController> logger;
        private readonly IProductionPlanService service;

        public ProductionPlanController(ILogger<ProductionPlanController> logger, IProductionPlanService service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpPost]
        public ActionResult<ProductionPlanResult> GetProductionPlan(ProductionPlanRequest request)
        {
            try
            {
                var result = service.ComputeProductionPlan(request);
                return Ok(result);
            }
            catch (ProductionPlanException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
