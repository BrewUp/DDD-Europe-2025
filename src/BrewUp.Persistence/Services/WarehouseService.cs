using BrewUp.Shared.CustomTypes;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Persistence.Services;

public sealed class WarehouseService([FromKeyedServices("warehouse")] IRepository repository) : IWarehouseService
{
    public async Task UpdateAvailabilityDueToProductionOrderAsync(Guid beerId, string beerName, Quantity quantity)
    {
        var aggregate = Entities.Warehouses.Availability.CreateAvailability(beerId, beerName, quantity);
        await repository.InsertAsync(aggregate.MapToSharedDto());
    }
}
