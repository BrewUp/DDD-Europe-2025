using BrewUp.Shared.CustomTypes;

namespace BrewUp.Persistence.Services;

public interface IWarehouseService
{
    Task UpdateAvailabilityDueToProductionOrderAsync(Guid beerId, string beerName, Quantity quantity,
        CancellationToken cancellationToken);
}
