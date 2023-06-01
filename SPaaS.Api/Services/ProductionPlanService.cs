using SPaaS.Api.Enums;
using SPaaS.Api.Exceptions;
using SPaaS.Api.Extensions;
using SPaaS.Api.Models;

namespace SPaaS.Api.Services
{
    public class ProductionPlanService : IProductionPlanService
    {
        public ProductionPlanResult ComputeProductionPlan(ProductionPlanRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                ValidateProductionPlanRequest(request);

                var productionPlanResult = new ProductionPlanResult();

                var powerPlantDetails = ParseRequestToPowerPlantDetails(request);

                powerPlantDetails = FilterPowerPlantDetails(powerPlantDetails, request.Load);
                ValidatePowerPlantsProductionForLoad(powerPlantDetails, request.Load);
                powerPlantDetails = SortPowerPlantDetailsByMeritOrder(powerPlantDetails, request.Load);

                productionPlanResult = BuildProductionPlanResult(productionPlanResult, powerPlantDetails.ToList(), request.Load);

                return productionPlanResult;
            }
            catch
            {
                throw;
            }
        }

        private static IEnumerable<PowerPlantDetails> ParseRequestToPowerPlantDetails(ProductionPlanRequest request)
        {
            foreach (var powerPlant in request.PowerPlants.Where(p => p.Efficiency > 0))
            {
                var details = new PowerPlantDetails
                {
                    Name = powerPlant.Name,
                    EnergyCost = decimal.MaxValue,
                    MinProduction = powerPlant.Pmin,
                    MaxProduction = powerPlant.Pmax
                };

                switch (powerPlant.PowerPlantType)
                {
                    case PowerPlantType.WindTurbine:
                        details.EnergyCost = decimal.Zero;
                        details.MaxProduction = powerPlant.Pmax.CalculatePositivePercentageValue(request.FuelsInfo.WindPercentage);
                        break;
                    case PowerPlantType.GasFired:
                        details.EnergyCost = request.FuelsInfo.GasCost / powerPlant.Efficiency;
                        break;
                    case PowerPlantType.Turbojet:
                        details.EnergyCost = request.FuelsInfo.KerosineCost / powerPlant.Efficiency;
                        break;
                    default:
                        throw new InvalidPowerPlantTypeException(powerPlant.Type);
                }

                yield return details;
            }
        }

        private static IEnumerable<PowerPlantDetails> FilterPowerPlantDetails(IEnumerable<PowerPlantDetails> powerPlantDetails, in decimal requestLoad)
        {
            var loadToProduce = requestLoad;

            var filtered = powerPlantDetails.Where(d =>
                d.MaxProduction > decimal.Zero
                && d.MinProduction <= loadToProduce);

            return filtered;
        }

        private static IEnumerable<PowerPlantDetails> SortPowerPlantDetailsByMeritOrder(IEnumerable<PowerPlantDetails> powerPlantDetails, in decimal requestLoad)
        {
            var loadToProduce = requestLoad;

            var sorted = powerPlantDetails
                .OrderBy(d => d.EnergyCost)
                .ThenByDescending(d => d.MaxProduction)
                .ThenBy(d => d.MinProduction);

            return sorted;
        }

        private static void ValidateProductionPlanRequest(ProductionPlanRequest request)
        {
            if (request.Load < 0) throw new InvalidLoadException("Request load must be greater than or equal to 0.");

            if (request.FuelsInfo == null) throw new InvalidFuelsException("Fuels cannot be null");

            if (request.PowerPlants == null) throw new InvalidPowerPlantException("List of power plants cannot be null.");

            if (request.PowerPlants.Count <= 0) throw new InvalidPowerPlantException("List of power plants cannot be empty.");

            if (request.FuelsInfo.WindPercentage < 0) throw new InvalidFuelsException("Wind percentage must be greater than or equal to 0.");

            if (request.FuelsInfo.GasCost < 0) throw new InvalidFuelsException("Gas cost must be a positive number.");

            if (request.FuelsInfo.KerosineCost < 0) throw new InvalidFuelsException("Kerosine cost must be a positive number.");

            if (request.PowerPlants.Any(p => p.Pmin < 0)) throw new InvalidPowerPlantException("Power plant minimum production must be a positive number.");

            if (request.PowerPlants.Any(p => p.Pmax < 0)) throw new InvalidPowerPlantException("Power plant maximum production must be a positive number.");

            if (request.PowerPlants.Any(p => p.Efficiency < 0)) throw new InvalidPowerPlantException("Power plant efficiency must be a positive number.");

            if (request.PowerPlants.Any(p => string.IsNullOrWhiteSpace(p.Name))) throw new InvalidPowerPlantException($"Power plant must have a name.");

            if (request.PowerPlants.Any(p => p.Pmin > p.Pmax)) throw new InvalidPowerPlantException("Power plant minimum production must be smaller than the maximum production.");
        }

        private static void ValidatePowerPlantsProductionForLoad(IEnumerable<PowerPlantDetails> powerPlantDetails, in decimal requestLoad)
        {
            if (!requestLoad.IsSmallerOrEqualToAnyInSubset(powerPlantDetails.Select(d => d.MinProduction).ToList()))
            {
                throw new MinimumProductionExceedsLoadException();
            }

            if (powerPlantDetails.Sum(d => d.MaxProduction) < requestLoad)
            {
                throw new InsufficientProductionSumException();
            }
        }

        private static ProductionPlanResult BuildProductionPlanResult(ProductionPlanResult productionPlanResult, IList<PowerPlantDetails> powerPlantDetails, in decimal requestLoad)
        {
            if (requestLoad == 0)
            {
                return productionPlanResult;
            }

            var remainingLoad = requestLoad;

            for (var i = 0; i < powerPlantDetails.Count; ++i)
            {
                var load = Math.Min(powerPlantDetails[i].MaxProduction, remainingLoad);

                productionPlanResult.Add(new ProductionPlanItem
                {
                    PowerPlantName = powerPlantDetails[i].Name,
                    LoadToProduce = load.RoundToOneDecimalPlace()
                });

                remainingLoad -= load;
                if (remainingLoad == decimal.Zero)
                {
                    return new ProductionPlanResult(productionPlanResult.Where(r => r.LoadToProduce > 0));
                }

                var nextPowerPlantDetails = powerPlantDetails[i + 1];
                if (nextPowerPlantDetails.MinProduction > remainingLoad)
                {
                    var overload = nextPowerPlantDetails.MinProduction - remainingLoad;
                    for (var j = i; j >= i; j++)
                    {
                        var currentExcess = Math.Min(overload, productionPlanResult[j].LoadToProduce);

                        productionPlanResult[j].LoadToProduce -= currentExcess.RoundToOneDecimalPlace();
                        remainingLoad += currentExcess;

                        if ((overload -= currentExcess) <= 0)
                        {
                            break;
                        }
                    }
                }
            }

            return productionPlanResult;
        }
    }
}