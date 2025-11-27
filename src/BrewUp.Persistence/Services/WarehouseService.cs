using BrewUp.Shared.CustomTypes;
using static BrewUp.Persistence.Entities.Warehouses.Availability;
using Availability = BrewUp.Shared.Entities.Availability;

namespace BrewUp.Persistence.Services;

public delegate Task InsertAvailability(Availability availability);
public delegate BrewUp.Persistence.Entities.Warehouses.Availability CreateAvailability(Guid beerId, string beerName, Quantity quantity);

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
