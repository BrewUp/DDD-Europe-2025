using BrewUp.Shared.CustomTypes;
using static BrewUp.Persistence.Warehouses.Availability;

namespace BrewUp.Persistence.Warehouses;

public delegate Task InsertAvailability(Shared.Entities.Availability availability);
public delegate Availability CreateAvailability(Guid beerId, string beerName, Quantity quantity);

public delegate Task UpdateAvailabilityDueToProductionOrder(
    Guid beerId,
    string beerName,
    Quantity quantity);

public static class WarehouseService
{
    public static UpdateAvailabilityDueToProductionOrder UpdateAvailabilityDueToProductionOrder(
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
