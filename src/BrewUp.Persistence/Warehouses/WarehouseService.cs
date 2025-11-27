using BrewUp.Shared.CustomTypes;
using static BrewUp.Persistence.Warehouses.Availability;

namespace BrewUp.Persistence.Warehouses;

internal delegate Task InsertAvailability(Shared.Entities.Availability availability);

internal delegate Availability CreateAvailability(Guid beerId, string beerName, Quantity quantity);

internal static class WarehouseService
{
    internal static UpdateAvailabilityDueToProductionOrder UpdateAvailabilityDueToProductionOrder(
        InsertAvailability insertAvailability) =>
        async (
            beerId,
            beerName,
            quantity) =>
        {
            var aggregate = CreateAvailability(beerId, beerName, quantity);
            await insertAvailability(aggregate.MapToSharedDto());
        };
}
