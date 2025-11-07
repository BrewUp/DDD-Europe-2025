using BrewUp.Shared.CustomTypes;

namespace BrewUp.Persistence.Services;

public interface IWarehouseService
{
    Task UpdateAvailabilityDueToProductionOrderAsync(BeerId beerId, BeerName beerName, Quantity quantity,
        CancellationToken cancellationToken);
}