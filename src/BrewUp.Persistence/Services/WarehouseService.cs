using BrewUp.Shared.CustomTypes;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Persistence.Services;

public sealed class WarehouseService : IWarehouseService
{
    private readonly IRepository _repository;

    public WarehouseService([FromKeyedServices("warehouse")] IRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateAvailabilityDueToProductionOrderAsync(Guid beerId, string beerName, Quantity quantity)
    {
        var aggregate = Entities.Warehouses.Availability.CreateAvailability(beerId, beerName, quantity);
        await _repository.InsertAsync(aggregate.MapToSharedDto());
    }
}
